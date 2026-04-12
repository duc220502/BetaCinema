using BetaCinema.Application.Common;
using BetaCinema.Application.Interfaces.AI;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Application.UseCases.AI
{
    public class AiModelClientResolver : IAiContextResolver
    {
        private readonly IReadOnlyDictionary<string, IGenerativeAiService> _clients;
        private readonly AiProviderOptions _options;

        public AiModelClientResolver(IEnumerable<IGenerativeAiService> clients, IOptions<AiProviderOptions> options)
        {
            _clients = clients.ToDictionary(x => x.ProviderName, StringComparer.OrdinalIgnoreCase);
            _options = options.Value;
        }

        public IGenerativeAiService Get(string providerName)
        {
            if (_clients.TryGetValue(providerName, out var client))
                return client;

            throw new InvalidOperationException($"AI provider '{providerName}' is not registered.");
        }

        public IGenerativeAiService GetDefault()
        {
            return Get(_options.DefaultProvider);
        }
    }
}
