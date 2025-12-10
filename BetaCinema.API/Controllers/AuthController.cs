using BetaCinema.Application.DTOs.DataRequest;
using BetaCinema.Application.DTOs.DataRequest.Users;
using BetaCinema.Application.DTOS.DataRequest.Users;
using BetaCinema.Application.Enums;
using BetaCinema.Application.Exceptions;
using BetaCinema.Application.Interfaces;
using BetaCinema.Application.Interfaces.Auths;
using BetaCinema.Application.UseCases.Users;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
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
            var o = mon.Get(scheme);
            var cfgMgr = o.ConfigurationManager ?? throw new  NotFoundException("ConfigurationManager null");

            try
            {
                // THÊM dòng này để force refresh
                cfgMgr?.RequestRefresh();

                var cfg = await cfgMgr!.GetConfigurationAsync(ct);
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
        public async Task<IActionResult> Callback([FromRoute] string provider, CancellationToken ct = default)
        {
            Console.WriteLine(">>> ENTERED Callback");

            var auth = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            if (!auth.Succeeded || auth.Principal is null)
                return Unauthorized();

           
            var returnUrl = auth.Properties?.Items["returnUrl"] ?? "/api/auth/dev/login-success";

            if (string.IsNullOrEmpty(returnUrl) || !Url.IsLocalUrl(returnUrl))
                returnUrl = "/api/auth/dev/login-success";

            var user =  await _externalAuthService.HandleCallbackAsync(provider, auth.Principal, ct);


            var token = await _tokenService.GenerateTokenAsync(user);

            Console.WriteLine($">>> Redirecting to: {returnUrl}");

            var separator = returnUrl.Contains('?') ? "&" : "?";

            return LocalRedirect($"{returnUrl}{separator}token={token.AccessToken}");
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
            // Debug: In tất cả claims
            Console.WriteLine("=== All Claims ===");
            foreach (var claim in User.Claims)
            {
                Console.WriteLine($"{claim.Type}: {claim.Value}");
            }

            var info = new
            {
                token,
                user = new
                {
                    // Thử nhiều cách lấy name
                    name = User.Identity?.Name
                        ?? User.FindFirst("name")?.Value
                        ?? User.FindFirst(ClaimTypes.Name)?.Value
                        ?? User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name")?.Value,

                    // Email
                    email = User.FindFirst("email")?.Value
                         ?? User.FindFirst(ClaimTypes.Email)?.Value
                         ?? User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress")?.Value,

                    // Sub
                    sub = User.FindFirst("sub")?.Value
                       ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                       ?? User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value,

                    // Thêm để debug
                    allClaims = User.Claims.Select(c => new { c.Type, c.Value }).ToList()
                }
            };
            return Ok(info);
        }


        [HttpPost("users")]
        public async Task<IActionResult> Register([FromBody] Request_Register rq)
        {
            var response = await _userRegistrationService.Register(rq,ConfirmationMethod.OTP);
            return StatusCode(StatusCodes.Status201Created, response);
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
