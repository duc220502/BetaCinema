using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Application.Interfaces
{
    public interface IRazorTemplateService
    {
        Task<string> RenderTemplateAsync<TViewModel>(string templateName, TViewModel viewModel);
    }
}

