using Intellimap.Core.Exceptions;
using Intellimap.Core.Infrastructure.Http;
using Intellimap.Core.Knowledge.Network;
using Intellimap.Core.Modules.Options;
using Intellimap.Core.Targets.Generic;
using System.Net;
using System.Net.Http.Json;

namespace Intellimap.Core.Modules.Standard.ShodanInternetDb
{
    /// <summary>
    /// Resolves internet-facing exposure data for an IP address using the Shodan InternetDB service.
    /// </summary>
    public sealed class ShodanInternetDbModule : AbstractModule<ShodanInternetDbModuleResult>
    {
        public override string ModuleId => "shodan-internetdb";

        public override IReadOnlyCollection<Type> SupportedTargetTypes =>
            [
                typeof(IpAddressTarget)
            ];

        public override IReadOnlyCollection<Type> ProducedKnowledgeTypes =>
            [
                typeof(ExposureKnowledge)
            ];

        protected override async Task<ShodanInternetDbModuleResult> ExecuteCoreAsync(ModuleExecutionContext context, CancellationToken cancellationToken)
        {
            if (context.Target is not IpAddressTarget ipTarget)
                throw new InvalidModuleTargetException(ModuleId, context.Target.GetType());

            var httpOptions = context.Options as IHttpOptions;
            var client = HttpClientCache.Get(httpOptions);

            var url = $"https://internetdb.shodan.io/{ipTarget.Address}";

            ShodanInternetDbResponse? response;
            try
            {
                using var httpResponse = await client.GetAsync(url, cancellationToken);

                if (httpResponse.StatusCode == HttpStatusCode.NotFound)
                {
                    response = null;
                }
                else
                {
                    httpResponse.EnsureSuccessStatusCode();
                    response = await httpResponse.Content.ReadFromJsonAsync<ShodanInternetDbResponse>(cancellationToken);
                }
            }
            catch (OperationCanceledException) when (context.CancellationToken.IsCancellationRequested)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new ModuleExecutionException(ModuleId, "Service call failed", ex);
            }

            var exposureKnowledge = new ExposureKnowledge
            {
                IpAddress = ipTarget.Address.ToString(),
                Found = response is not null,
                Ports = response?.Ports ?? [],
                Hostnames = response?.Hostnames ?? [],
                Cpes = response?.Cpes ?? [],
                Tags = response?.Tags ?? [],
                Vulnerabilities = response?.Vulns ?? []
            };

            var result = CreateResult();
            result.ExposureKnowledge = exposureKnowledge;
            return result;
        }
    }
}
