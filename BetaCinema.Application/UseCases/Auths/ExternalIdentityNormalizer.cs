using BetaCinema.Application.DTOs;
using BetaCinema.Application.Interfaces.Auths;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Application.UseCases.Auths
{
    public class ExternalIdentityNormalizer : IExternalIdentityNormalizer
    {
        public ExternalIdentity Normalize(ClaimsPrincipal p, string provider)
        {
            string sub, email, name;

            Console.WriteLine($"=== Normalizing claims for {provider} ===");

            if (provider.Equals("facebook", StringComparison.OrdinalIgnoreCase))
            {

                sub = p.FindFirst("id")?.Value
                   ?? p.FindFirst(ClaimTypes.NameIdentifier)?.Value
                   ?? p.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value
                   ?? "";

                Console.WriteLine($"  Facebook ID (sub): {sub}");

                email = p.FindFirst(ClaimTypes.Email)?.Value
                     ?? p.FindFirst("email")?.Value
                     ?? p.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress")?.Value
                     ?? "";

                Console.WriteLine($"  Facebook Email: {email}");

                name = p.FindFirst(ClaimTypes.Name)?.Value
                    ?? p.FindFirst("name")?.Value
                    ?? p.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name")?.Value
                    ?? ""; 

                Console.WriteLine($"  Facebook Name: {name}");

               
                if (string.IsNullOrEmpty(sub))
                {
                    Console.WriteLine("  ❌ ERROR: Facebook ID (sub) is empty!");
                    Console.WriteLine("  Available claims:");
                    foreach (var claim in p.Claims)
                    {
                        Console.WriteLine($"    {claim.Type}: {claim.Value}");
                    }
                }
            }
            else 
            {
                sub = p.FindFirst("sub")?.Value
                   ?? p.FindFirst(ClaimTypes.NameIdentifier)?.Value
                   ?? p.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value
                   ?? "";

                Console.WriteLine($"  Google Sub: {sub}");

                email = p.FindFirst(ClaimTypes.Email)?.Value
                     ?? p.FindFirst("email")?.Value
                     ?? "";

                Console.WriteLine($"  Google Email: {email}");

                name = p.FindFirst(ClaimTypes.Name)?.Value
                    ?? p.FindFirst("name")?.Value
                    ?? email;

                Console.WriteLine($"  Google Name: {name}");
            }

            var result = new ExternalIdentity
            {
                Provider = provider,
                ProviderKey = sub,  
                Email = email,
                Name = name
            };

            Console.WriteLine($"=== Normalized Result ===");
            Console.WriteLine($"  Provider: {result.Provider}");
            Console.WriteLine($"  ProviderKey: {result.ProviderKey}");
            Console.WriteLine($"  Email: {result.Email}");
            Console.WriteLine($"  Name: {result.Name}");

            return result;

        }
    }
}
