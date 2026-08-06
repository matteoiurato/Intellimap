using Intellimap.Core.Exceptions;
using Intellimap.Core.Infrastructure.Http;
using Intellimap.Core.Knowledge.Network;
using Intellimap.Core.Modules.Options;
using Intellimap.Core.Targets.Generic;
using System.Net.Http.Json;

namespace Intellimap.Core.Modules.Standard.CrtSh
{
    /// <summary>
    /// Resolves domains related to a hostname using the crt.sh certificate transparency search.
    /// </summary>
    public sealed class CrtShModule : AbstractModule<CrtShModuleResult>
    {
        public override string ModuleId => "crt-sh";

        public override IReadOnlyCollection<Type> SupportedTargetTypes =>
            [
                typeof(HostnameTarget)
            ];

        public override IReadOnlyCollection<Type> ProducedKnowledgeTypes =>
            [
                typeof(DomainKnowledge)
            ];

        protected override async Task<CrtShModuleResult> ExecuteCoreAsync(ModuleExecutionContext context, CancellationToken cancellationToken)
        {
            if (context.Target is not HostnameTarget hostnameTarget)
                throw new InvalidModuleTargetException(ModuleId, context.Target.GetType());

            var httpOptions = context.Options as IHttpOptions;
            var client = HttpClientCache.Get(httpOptions);

            var url = $"https://crt.sh/?q={Uri.EscapeDataString(hostnameTarget.Hostname)}&output=json";

            List<CrtShEntry>? entries;
            try
            {
                entries = await client.GetFromJsonAsync<List<CrtShEntry>>(url, cancellationToken);
            }
            catch (OperationCanceledException) when (context.CancellationToken.IsCancellationRequested)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new ModuleExecutionException(ModuleId, "Service call failed", ex);
            }

            entries ??= [];

            var relatedDomains = ExtractRelatedDomains(entries);

            var domainKnowledge = new DomainKnowledge
            {
                Hostname = hostnameTarget.Hostname,
                Found = relatedDomains.Count > 0,
                RelatedDomains = relatedDomains
            };

            var result = CreateResult();
            result.DomainKnowledge = domainKnowledge;
            return result;
        }

        /// <summary>
        /// Extracts the deduplicated, normalized set of domain names referenced across all certificate entries.
        /// crt.sh's name field is not restricted to DNS names, so non-domain values (email SANs, free-text entries from misissued or test certificates) are filtered out.
        /// </summary>
        private static List<string> ExtractRelatedDomains(List<CrtShEntry> entries)
        {
            return entries
                .SelectMany(e => e.NameValue.Split('\n', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
                .Select(NormalizeDomain)
                .Where(d => Uri.CheckHostName(d) == UriHostNameType.Dns)
                .Distinct()
                .OrderBy(d => d, StringComparer.Ordinal)
                .ToList();
        }

        /// <summary>
        /// Strips a leading wildcard marker and lowercases a domain name for consistent deduplication.
        /// </summary>
        private static string NormalizeDomain(string domain)
        {
            if (domain.StartsWith("*.", StringComparison.Ordinal))
                domain = domain[2..];

            return domain.ToLowerInvariant();
        }
    }
}
