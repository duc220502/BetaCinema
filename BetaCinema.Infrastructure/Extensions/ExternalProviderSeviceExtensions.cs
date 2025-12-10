using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Http;
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
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Infrastructure.Extensions
{
    public static class ExternalProviderSeviceExtensions
    {

        public static AuthenticationBuilder AddExternalProviders(this AuthenticationBuilder authBuilder, IConfiguration cfg)
        {
            var providers = cfg.GetSection("Authentication:Providers");
            if (!providers.Exists()) return authBuilder;

            

            foreach (var p in providers.GetChildren())
            {
                var name = p.Key;                       
                var type = p["Type"]?.ToLowerInvariant();

                switch (type)
                {
                    case "openidconnect":


                        authBuilder.AddOpenIdConnect(name, o =>
                        {
                            o.Authority = "https://accounts.google.com";

                           
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

                        if (name == "facebook")
                        {
                            Console.WriteLine($"Adding Facebook OAuth for {name}");
                            authBuilder.AddFacebook(name, o =>  
                            {
                                o.AppId = p["ClientId"]!;
                                o.AppSecret = p["ClientSecret"]!;
                                o.CallbackPath = new PathString(p["CallbackPath"] ?? $"/oauth2/callback/{name}");

                                foreach (var s in p.GetSection("Scopes").Get<string[]>() ?? [])
                                    o.Scope.Add(s);

                                o.SaveTokens = true;
                                o.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;

                                var claims = p.GetSection("ClaimMap").Get<Dictionary<string, string>>() ?? new();

                                o.Fields.Clear(); 
                                o.Fields.Add("id");
                                o.Fields.Add("name");
                                o.Fields.Add("email");
                                o.Fields.Add("picture");

                                // Map claims
                                foreach (var kv in claims)
                                    o.ClaimActions.MapJsonKey(kv.Key, kv.Value);

                                o.Events = new OAuthEvents
                                {
                                    OnCreatingTicket = ctx =>
                                    {
                                        Console.WriteLine("[Facebook] OnCreatingTicket");

                                        
                                        var json = ctx.User.GetRawText();
                                        Console.WriteLine($"Raw Facebook JSON: {json}");

                                        
                                        Console.WriteLine("Claims:");
                                        foreach (var claim in ctx.Principal?.Claims ?? Enumerable.Empty<Claim>())
                                        {
                                            Console.WriteLine($"    {claim.Type}: {claim.Value}");
                                        }

                                        return Task.CompletedTask;
                                    },
                                    OnRemoteFailure = ctx =>
                                    {
                                        Console.WriteLine($"[Facebook] RemoteFailure: {ctx.Failure}");
                                        ctx.Response.Redirect("/");
                                        ctx.HandleResponse();
                                        return Task.CompletedTask;
                                    }
                                };
                            });
                        }
                        else
                        {
                            authBuilder.AddOAuth(name, o =>
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

                                o.SaveTokens = true;
                                o.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;

                                foreach (var kv in claims)
                                    o.ClaimActions.MapJsonKey(kv.Key, kv.Value);



                                o.Events = new OAuthEvents
                                {
                                    OnCreatingTicket = ctx =>
                                    {
                                        Console.WriteLine($"[Facebook] Ticket created");
                                        return Task.CompletedTask;
                                    }
                                };
                            });
                        }

                            
                        break;

                    default:
                        throw new NotSupportedException($"Unknown auth provider type: {type} for {name}");
                }
            }

            return authBuilder;
        }
    }
}
