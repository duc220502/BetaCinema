using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Protocols;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Infrastructure.Extensions
{
    public static class ExternalProviderSeviceExtensions
    {

        public static IServiceCollection AddExternalProviders(this IServiceCollection services, IConfiguration cfg)
        {
            var providers = cfg.GetSection("Authentication:Providers");
            if (!providers.Exists()) return services;

            var auth = services.AddAuthentication();

            foreach (var p in providers.GetChildren())
            {
                var name = p.Key;                       
                var type = p["Type"]?.ToLowerInvariant();

                switch (type)
                {
                    case "openidconnect":
                        

                        auth.AddOpenIdConnect(name, o =>
                        {
                            o.Authority = "https://accounts.google.com";
                            o.MetadataAddress = "https://accounts.google.com/.well-known/openid-configuration";

                            o.ClientId = p["ClientId"]!;
                            o.ClientSecret = p["ClientSecret"]!;
                            o.CallbackPath = p["CallbackPath"] ?? "/oauth2/callback/google";

                            o.ResponseType = OpenIdConnectResponseType.Code;
                            o.UsePkce = true;

                            o.Scope.Clear();
                            o.Scope.Add("openid"); o.Scope.Add("email"); o.Scope.Add("profile");

                            o.GetClaimsFromUserInfoEndpoint = true;
                            o.SaveTokens = true;
                            o.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                            o.RequireHttpsMetadata = true;

                            o.Events = new OpenIdConnectEvents
                            {
                                OnRedirectToIdentityProvider = ctx =>
                                {
                                    Console.WriteLine($"[OIDC:{ctx.Scheme.Name}] Redirecting to: {ctx.ProtocolMessage.CreateAuthenticationRequestUrl()}");
                                    return Task.CompletedTask;
                                },
                                OnAuthenticationFailed = ctx =>
                                {
                                    Console.WriteLine($"[OIDC:{ctx.Scheme.Name}] AuthFailed: {ctx.Exception}");
                                    return Task.CompletedTask;
                                }
                            };

                        });
                        break;






                    case "oauth2":
                        auth.AddOAuth(name, o =>
                        {
                            o.ClientId = p["ClientId"]!;
                            o.ClientSecret = p["ClientSecret"]!;
                            o.CallbackPath = p["CallbackPath"]!;
                            o.AuthorizationEndpoint = p["AuthorizationEndpoint"]!;
                            o.TokenEndpoint = p["TokenEndpoint"]!;
                            o.UserInformationEndpoint = p["UserInformationEndpoint"]!;
                            foreach (var s in p.GetSection("Scopes").Get<string[]>() ?? [])
                                o.Scope.Add(s);

                            
                            var claims = p.GetSection("ClaimMap").Get<Dictionary<string, string>>() ?? new();
                            foreach (var kv in claims)
                                o.ClaimActions.MapJsonKey(kv.Key, kv.Value);

                            o.Events = new OAuthEvents
                            {
                                OnCreatingTicket = async ctx =>
                                {
                                    if (!string.IsNullOrEmpty(ctx.Options.UserInformationEndpoint))
                                    {
                                        var req = new HttpRequestMessage(HttpMethod.Get, ctx.Options.UserInformationEndpoint);
                                        req.Headers.Authorization =
                                            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", ctx.AccessToken);
                                        var resp = await ctx.Backchannel.SendAsync(req);
                                        resp.EnsureSuccessStatusCode();
                                        using var doc = await System.Text.Json.JsonDocument.ParseAsync(await resp.Content.ReadAsStreamAsync());
                                        ctx.RunClaimActions(doc.RootElement);
                                    }
                                }
                            };
                        });
                        break;

                    default:
                        throw new NotSupportedException($"Unknown auth provider type: {type} for {name}");
                }
            }

            return services;
        }
    }
}
