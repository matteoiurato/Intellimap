using Intellimap.Core.Knowledge;
using Intellimap.Core.Knowledge.Network;

namespace Intellimap.Core.Modules.Standard.IpApi
{
    /// <summary>
    /// Result produced by <see cref="IpApiModule"/>.
    /// </summary>
    public class IpApiModuleResult : AbstractModuleResult
    {
        public IpAddressKnowledge IpAddressKnowledge { get; set; } = null!;

        public override IReadOnlyList<IKnowledge> Knowledge => [
            IpAddressKnowledge
        ];
    }
}