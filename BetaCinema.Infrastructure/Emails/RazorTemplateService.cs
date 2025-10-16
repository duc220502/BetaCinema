using BetaCinema.Application.DTOs;
using BetaCinema.Application.Interfaces;
using RazorEngineCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Infrastructure.Emails
{
    public class RazorTemplateService : IRazorTemplateService
    {
        public async Task<string> RenderTemplateAsync<TViewModel>(string templateName, TViewModel viewModel)
        {
            var templateContent = await ReadTemplateContentAsync(templateName);

            var razorEngine = new RazorEngine();

            var compiledTemplate = await razorEngine.CompileAsync(templateContent, builder =>
            {
                // Add references needed by the template
                builder.AddAssemblyReference(typeof(object).Assembly);
                builder.AddAssemblyReference(typeof(Enumerable).Assembly);

                // IMPORTANT: add the assembly that contains your DTO/ViewModel types

                builder.AddAssemblyReference(typeof(TViewModel).Assembly);

                // also add your infrastructure assembly if template references types there
                builder.AddAssemblyReference(Assembly.GetExecutingAssembly());

                builder.AddUsing("System");
                builder.AddUsing("System.Linq");
            });

            return await compiledTemplate.RunAsync(viewModel);
        }

        private async Task<string> ReadTemplateContentAsync(string templateName)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = $"BetaCinema.Infrastructure.Emails.Templates.{templateName}";

            using var stream = assembly.GetManifestResourceStream(resourceName)
                ?? throw new FileNotFoundException(
                    $"Không thể tìm thấy template nhúng: '{resourceName}'. " +
                    $"Hãy chắc chắn rằng Build Action của file .cshtml là 'Embedded resource'.");

            using var reader = new StreamReader(stream);
            return await reader.ReadToEndAsync();
        }
    }
}
