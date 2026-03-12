using BetaCinema.Application.DTOs.Auth.External;
using BetaCinema.Application.Interfaces.Catching;
using Microsoft.Extensions.Configuration;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BetaCinema.Infrastructure.Catching.Redis
{
    public class RedisExternalLinkingStore(IConnectionMultiplexer mux, IConfiguration config) : IExternalLinkingStore
    {
        private readonly IDatabase _db = mux.GetDatabase();
        private readonly string _otpSalt = config["Security:OtpSalt"] ?? "CHANGE_ME_OTP_SALT";

        private static string StateKey(string token) => $"extlink:state:{token}";
        private static string OtpKey(string token) => $"extlink:otp:{token}";
        private static string FailKey(string token) => $"extlink:fail:{token}";
        private static string UsedKey(string token) => $"extlink:used:{token}";


        public async Task SaveStateAsync(string linkingToken, ExternalLinkState state, TimeSpan ttl, CancellationToken ct)
        {
            var json = JsonSerializer.Serialize(state);
            await _db.StringSetAsync(StateKey(linkingToken), json, ttl);
        }

        public async Task<ExternalLinkState?> GetStateAsync(string linkingToken, CancellationToken ct)
        {
            var value = await _db.StringGetAsync(StateKey(linkingToken));
            if (value.IsNullOrEmpty) return null;

            return JsonSerializer.Deserialize<ExternalLinkState>(value!);
        }

        public async Task DeleteStateAsync(string linkingToken, CancellationToken ct)
        {
            await _db.KeyDeleteAsync(StateKey(linkingToken));
        }


        public async Task SaveOtpAsync(string linkingToken, string otpPlain, TimeSpan ttl, CancellationToken ct)
        {
            var hash = HashOtp(linkingToken, otpPlain);

            await _db.StringSetAsync(OtpKey(linkingToken), hash, ttl);

            // reset fail count + used flag khi gửi OTP mới
            await _db.KeyDeleteAsync(FailKey(linkingToken));
            await _db.KeyDeleteAsync(UsedKey(linkingToken));
        }

        public async Task<bool> VerifyOtpAsync(string linkingToken, string otpPlain, CancellationToken ct)
        {
            var stored = await _db.StringGetAsync(OtpKey(linkingToken));
            if (stored.IsNullOrEmpty) return false;

            var provided = HashOtp(linkingToken, otpPlain);

            var storedBytes = Encoding.UTF8.GetBytes(stored!);
            var providedBytes = Encoding.UTF8.GetBytes(provided);

            return storedBytes.Length == providedBytes.Length &&
                   CryptographicOperations.FixedTimeEquals(storedBytes, providedBytes);
        }


        public async Task<long> IncrementFailCountAsync(string linkingToken, CancellationToken ct)
        {
            var key = FailKey(linkingToken);

            var n = await _db.StringIncrementAsync(key);

            if (n == 1)
            {
                await _db.KeyExpireAsync(key, TimeSpan.FromMinutes(10));
            }

            return n;
        }
        public async Task MarkUsedAsync(string linkingToken, CancellationToken ct)
        {
            await _db.StringSetAsync(
                UsedKey(linkingToken),
                "1",
                TimeSpan.FromMinutes(10));
        }

        public async Task<bool> IsUsedAsync(string linkingToken, CancellationToken ct)
        {
            var value = await _db.StringGetAsync(UsedKey(linkingToken));
            return value == "1";
        }

        private string HashOtp(string linkingToken, string otpPlain)
        {
            var input = $"{linkingToken}:{otpPlain}:{_otpSalt}";
            var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(input));
            return Convert.ToHexString(bytes);
        }


    }
}
