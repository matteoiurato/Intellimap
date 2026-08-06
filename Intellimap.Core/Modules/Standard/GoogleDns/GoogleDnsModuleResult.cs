using Intellimap.Core.Knowledge;
using Intellimap.Core.Knowledge.Network;

namespace Intellimap.Core.Modules.Standard.GoogleDns
{
    /// <summary>
    /// Result produced by <see cref="GoogleDnsModule"/>.
    /// </summary>
    public class GoogleDnsModuleResult : AbstractModuleResult
    {
        public DnsKnowledge DnsKnowledge { get; set; } = null!;

        public override IReadOnlyList<AbstractKnowledge> Knowledge => [
            DnsKnowledge
        ];
    }
}