using BetaCinema.Application.DTOs.DataRequest;
using BetaCinema.Application.DTOs.DataRequest.Users;
using BetaCinema.Application.DTOS.DataRequest.Users;
using BetaCinema.Application.Enums;
using BetaCinema.Application.Interfaces;
using BetaCinema.Application.Interfaces.Auths;
using BetaCinema.Application.UseCases.Users;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace BetaCinema.API.Controllers
{
    [Route("api")]
    [ApiController]
    public class AuthController(IAuthService authService , IUserRegistrationService userRegistrationService , 
    ITokenService tokenService , IExternalAuthService externalAuthService , IAuthenticationSchemeProvider schemes) : ControllerBase
    {
        private readonly IUserRegistrationService _userRegistrationService = userRegistrationService;
        private readonly IAuthService _authService = authService;
        private readonly ITokenService _tokenService = tokenService;
        private readonly IExternalAuthService _externalAuthService = externalAuthService;
        private readonly IAuthenticationSchemeProvider _schemes = schemes;

        [HttpGet("api/auth/debug/assemblies")]
        public IActionResult Assemblies()
        {
            string A(Type t) => t.Assembly.FullName ?? "(null)";

            var list = new[]
            {
        new { Name = "OpenIdConnectHandler", Assembly = A(typeof(Microsoft.AspNetCore.Authentication.OpenIdConnect.OpenIdConnectHandler)) },
        new { Name = "JwtBearerHandler",     Assembly = A(typeof(Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerHandler)) },
        new { Name = "IM.Protocols",         Assembly = A(typeof(Microsoft.IdentityModel.Protocols.IDocumentRetriever)) },
        new { Name = "IM.Protocols.OIDC",    Assembly = A(typeof(Microsoft.IdentityModel.Protocols.OpenIdConnect.OpenIdConnectConfiguration)) },
        new { Name = "IM.Tokens",            Assembly = A(typeof(Microsoft.IdentityModel.Tokens.SecurityKey)) },
        new { Name = "System.IdentityModel", Assembly = A(typeof(System.IdentityModel.Tokens.Jwt.JwtSecurityToken)) },
    };

            return Ok(list);
        }

        [HttpGet("api/auth/debug/oidc-config/{scheme}")]
        public async Task<IActionResult> OidcBackchannelConfig(
        string scheme,
        [FromServices] IOptionsMonitor<OpenIdConnectOptions> mon,
        CancellationToken ct)
        {
            var o = mon.Get(scheme); // "google"
                                     // Cực quan trọng: dùng ConfigurationManager của chính OIDC
            var cfgMgr = o.ConfigurationManager;
            try
            {
                var cfg = await cfgMgr.GetConfigurationAsync(ct);
                return Ok(new
                {
                    scheme,
                    from = "OpenIdConnectOptions.ConfigurationManager",
                    authorization_endpoint = cfg.AuthorizationEndpoint,
                    token_endpoint = cfg.TokenEndpoint,
                    userinfo_endpoint = cfg.UserInfoEndpoint,
                    issuer = cfg.Issuer,
                    jwks_uri = cfg.JwksUri
                });
            }
            catch (Exception ex)
            {
                return Problem(title: "Discovery via OIDC backchannel FAILED",
                               detail: ex.ToString());
            }
        }

        [HttpGet("api/auth/debug/oidc-raw/{scheme}")]
        public async Task<IActionResult> OidcRaw(string scheme, [FromServices] IOptionsMonitor<OpenIdConnectOptions> mon)
        {
            var o = mon.Get(scheme);
            var http = o.Backchannel ?? new HttpClient();
            var url = o.MetadataAddress ?? "https://accounts.google.com/.well-known/openid-configuration";
            var resp = await http.GetAsync(url);
            var text = await resp.Content.ReadAsStringAsync();
            return Ok(new { url, status = (int)resp.StatusCode, ok = resp.IsSuccessStatusCode, text });
        }

        [HttpGet("api/auth/debug/discovery")]
        public async Task<IActionResult> Discovery()
        {
            var url = "https://accounts.google.com/.well-known/openid-configuration";
            using var http = new HttpClient();
            var resp = await http.GetAsync(url);
            var ok = resp.IsSuccessStatusCode;
            var text = await resp.Content.ReadAsStringAsync();
            return Ok(new { url, status = (int)resp.StatusCode, ok, snippet = text[..Math.Min(text.Length, 200)] });
        }

        [HttpGet("api/auth/debug/oidc/{scheme}")]
        public IActionResult OidcOptions(string scheme, [FromServices] IOptionsMonitor<OpenIdConnectOptions> mon)
        {
            var o = mon.Get(scheme); // "google"
            return Ok(new
            {
                Scheme = scheme,
                Authority = o.Authority,
                ClientId = o.ClientId,
                HasClientSecret = !string.IsNullOrEmpty(o.ClientSecret),
                CallbackPath = o.CallbackPath.Value,
                ResponseType = o.ResponseType,
                Scopes = o.Scope.ToArray(),
                SaveTokens = o.SaveTokens,
                GetClaimsFromUserInfoEndpoint = o.GetClaimsFromUserInfoEndpoint
            });
        }
        [HttpGet("auth/{provider}/login")]
        public async Task<IActionResult> Login([FromRoute] string provider, [FromQuery] string returnUrl = "/api/auth/dev/login-success")
        {
            if ((await _schemes.GetSchemeAsync(provider)) is null) return NotFound();
            if (string.IsNullOrEmpty(returnUrl) || !Url.IsLocalUrl(returnUrl)) returnUrl = "/";

            var props = new AuthenticationProperties
            {
                RedirectUri = Url.Content($"/api/auth/{provider}/callback")
            };
            props.Items["returnUrl"] = returnUrl;



            return Challenge(props, provider);
        }

        [HttpGet("auth/{provider}/callback")]
        public async Task<IActionResult> Callback([FromRoute] string provider, [FromQuery] string? returnUrl = "/", CancellationToken ct = default)
        {
            Console.WriteLine(">>> ENTERED Callback");

            if (string.IsNullOrEmpty(returnUrl) || !Url.IsLocalUrl(returnUrl)) returnUrl = "/";
            var auth = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            if (!auth.Succeeded || auth.Principal is null) return Unauthorized();

            await _externalAuthService.HandleCallbackAsync(provider, auth.Principal, ct);
            return LocalRedirect(returnUrl);
        }

        [HttpPost("auth/login")]
        public async Task<IActionResult> Login([FromBody] Request_Login rq)
        {

            var response = await _authService.Login(rq);
            return Ok(response);
        }


        [AllowAnonymous]
        [HttpGet("auth/dev/login-success")]
        public IActionResult DevLoginSuccess([FromQuery] string? token = null)
        {
            var info = new
            {
                token,
                user = new
                {
                    name = User.Identity?.Name,
                    email = User.Claims.FirstOrDefault(c => c.Type == "email")?.Value
                         ?? User.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.Email)?.Value,
                    sub = User.Claims.FirstOrDefault(c => c.Type == "sub")?.Value
                }
            };
            return Ok(info);
        }


        [HttpPost("users")]
        public async Task<IActionResult> Register([FromBody] Request_Register rq)
        {
            var user = await _userRegistrationService.Register(rq,ConfirmationMethod.OTP);
            return CreatedAtAction(nameof(UsersController.GetUserById), "Users", new { id = user?.Data?.Id }, user);
        }

        [HttpPut("auth/email-verifications")]
        public async Task<IActionResult> ConfirmEmail([FromBody] Request_ConfirmEmailRegister rq)
        {
            var response = await _userRegistrationService.ConfirmEmailRegister(rq);
            return Ok(response);
        }

        [HttpPost("auth/renew")]
        public async Task<IActionResult> RenewToken([FromBody] Request_RenewToken rq)
        {
            var response = await _tokenService.RenewToken(rq);
            return Ok(response);
        }

        [HttpPost("auth/password-reset-requests")]
        public async Task<IActionResult> RequestResetPassword([FromBody] string account)
        {
            var response = await _authService.SendMailResetPasswordAsync(account, ConfirmationMethod.OTP);
            return Ok(response);
        }

        [HttpPost("auth/password-reset-verifications")]
        public async Task<IActionResult> VerifyResetCode([FromBody] Request_VerifyCode rq)
        {
            var response = await _authService.VerifyResetCodeAsync(rq);
            return Ok(response);
        }


        [HttpPut("auth/password-reset")]
        [Authorize(Policy = "PasswordReset")] 
        public async Task<IActionResult> ResetPassword([FromBody] Request_ResetPassword rq)
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(userIdString, out var userId))
            {
                return Unauthorized("Token không hợp lệ.");
            }

           var response =  await _authService.ResetPasswordAsync(userId, rq);

            return Ok(response);
        }
        
    }
}
