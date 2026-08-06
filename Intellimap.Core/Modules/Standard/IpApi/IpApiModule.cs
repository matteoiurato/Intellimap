using Intellimap.Core.Exceptions;
using Intellimap.Core.Infrastructure.Http;
using Intellimap.Core.Knowledge.Location;
using Intellimap.Core.Knowledge.Network;
using Intellimap.Core.Modules.Options;
using Intellimap.Core.Targets.Generic;
using System.Net.Http.Json;

namespace Intellimap.Core.Modules.Standard.IpApi
{
    /// <summary>
    /// Resolves geolocation and network ownership information for an IP address using the ip-api.com API.
    /// </summary>
    public sealed class IpApiModule : AbstractModule<IpApiModuleResult>
    {
        public override string ModuleId => "ip-api";

        public override IReadOnlyCollection<Type> SupportedTargetTypes =>
            [
                typeof(IpAddressTarget)
            ];

        public override IReadOnlyCollection<Type> ProducedKnowledgeTypes =>
            [
                typeof(IpAddressKnowledge)
            ];

        protected override async Task<IpApiModuleResult> ExecuteCoreAsync(ModuleExecutionContext context, CancellationToken cancellationToken)
        {
            if (context.Target is not IpAddressTarget ipTarget)
                throw new InvalidModuleTargetException(ModuleId, context.Target.GetType());

            var httpOptions = context.Options as IHttpOptions;
            var client = HttpClientCache.Get(httpOptions);

            var url = $"http://ip-api.com/json/{ipTarget.Address}";

            IpApiResponse? response;
            try
            {
                response = await client.GetFromJsonAsync<IpApiResponse>(url, cancellationToken);
            }
            catch (OperationCanceledException) when (context.CancellationToken.IsCancellationRequested)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new ModuleExecutionException(ModuleId, "Service call failed", ex);
            }

            if (response is null)
                throw new ModuleExecutionException(ModuleId, "No response from service");

            if (response.Status != "success")
                throw new ModuleExecutionException(ModuleId, $"Error response from service");

            var ipAddressKnowledge = new IpAddressKnowledge
            {
                IpAddress = response.Query,

                Location = new LocationKnowledge
                {
                    Country = response.Country,
                    CountryCode = response.CountryCode,
                    Region = response.Region,
                    RegionName = response.RegionName,
                    City = response.City,
                    PostalCode = response.Zip,
                    Latitude = response.Lat,
                    Longitude = response.Lon
                },

                NetworkOrganization = new NetworkOrganizationKnowledge
                {
                    Name = response.Org,
                    Provider = response.Isp,
                    AutonomousSystem = response.As
                },

                Timezone = response.Timezone
            };

            var result = CreateResult();
            result.IpAddressKnowledge = ipAddressKnowledge;
            return result;
        }
    }
}
