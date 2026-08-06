using Intellimap.Core.Exceptions;
using Intellimap.Core.Infrastructure.Http;
using Intellimap.Core.Knowledge.Network;
using Intellimap.Core.Modules.Options;
using Intellimap.Core.Targets.Generic;
using System.Net.Http.Json;

namespace Intellimap.Core.Modules.Standard.GoogleDns
{
    /// <summary>
    /// Resolves DNS records for a hostname using the Google DNS-over-HTTPS API.
    /// </summary>
    public sealed class GoogleDnsModule : AbstractModule<GoogleDnsModuleResult>
    {
        public override string ModuleId => "google-dns";

        public override IReadOnlyCollection<Type> SupportedTargetTypes =>
            [
                typeof(HostnameTarget)
            ];

        public override IReadOnlyCollection<Type> ProducedKnowledgeTypes =>
            [
                typeof(DnsKnowledge)
            ];

        private static readonly string[] QueriedTypes = ["A", "AAAA", "CNAME", "NS", "TXT", "MX"];

        protected override async Task<GoogleDnsModuleResult> ExecuteCoreAsync(ModuleExecutionContext context, CancellationToken cancellationToken)
        {
            if (context.Target is not HostnameTarget hostnameTarget)
                throw new InvalidModuleTargetException(ModuleId, context.Target.GetType());

            var httpOptions = context.Options as IHttpOptions;
            var client = HttpClientCache.Get(httpOptions);

            DnsResolveResponse?[] responses;
            try
            {
                var tasks = QueriedTypes.Select(type => QueryAsync(client, hostnameTarget.Hostname, type, cancellationToken));
                responses = await Task.WhenAll(tasks);
            }
            catch (OperationCanceledException) when (context.CancellationToken.IsCancellationRequested)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new ModuleExecutionException(ModuleId, "Service call failed", ex);
            }

            // Status 3 = NXDOMAIN. A domain that exists but simply lacks an A record
            // returns NOERROR with an empty answer list instead, so this is a reliable
            // signal that the domain doesn't exist at all, not just that it lacks this record type.
            var domainExists = responses[0]?.Status != 3;

            var dnsKnowledge = new DnsKnowledge
            {
                Hostname = hostnameTarget.Hostname,
                Found = domainExists
            };

            for (var i = 0; i < QueriedTypes.Length; i++)
            {
                if (responses[i]?.Status != 0) continue;

                var answers = responses[i]?.Answer;
                if (answers is null) continue;

                switch (QueriedTypes[i])
                {
                    case "A":
                        dnsKnowledge.ARecords = answers.Select(a => a.Data).ToList();
                        break;
                    case "AAAA":
                        dnsKnowledge.AaaaRecords = answers.Select(a => a.Data).ToList();
                        break;
                    case "CNAME":
                        dnsKnowledge.CnameRecords = answers.Select(a => a.Data.TrimEnd('.')).ToList();
                        break;
                    case "NS":
                        dnsKnowledge.NsRecords = answers.Select(a => a.Data.TrimEnd('.')).ToList();
                        break;
                    case "TXT":
                        dnsKnowledge.TxtRecords = answers.Select(a => a.Data.Trim('"')).ToList();
                        break;
                    case "MX":
                        dnsKnowledge.MxRecords = answers.Select(ParseMxRecord).ToList();
                        break;
                }
            }

            var result = CreateResult();
            result.DnsKnowledge = dnsKnowledge;
            return result;
        }

        /// <summary>
        /// Queries a single DNS record type for a hostname, returning the raw response status and all.
        /// </summary>
        private static async Task<DnsResolveResponse?> QueryAsync(HttpClient client, string hostname, string recordType, CancellationToken token)
        {
            var url = $"https://dns.google/resolve?name={Uri.EscapeDataString(hostname)}&type={recordType}";
            return await client.GetFromJsonAsync<DnsResolveResponse>(url, token);
        }

        /// <summary>
        /// Parses an MX record's priority and exchange from its raw answer data.
        /// </summary>
        private static MxRecord ParseMxRecord(DnsAnswer answer)
        {
            // Data format: "10 mail.example.com."
            var parts = answer.Data.Split(' ', 2);
            var priority = parts.Length == 2 && int.TryParse(parts[0], out var p) ? p : 0;
            var exchange = (parts.Length == 2 ? parts[1] : answer.Data).TrimEnd('.');
            return new MxRecord(exchange, priority);
        }
    }
}
