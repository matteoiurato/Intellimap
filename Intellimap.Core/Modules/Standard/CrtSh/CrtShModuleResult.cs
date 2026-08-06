using Intellimap.Core.Knowledge;
using Intellimap.Core.Knowledge.Network;

namespace Intellimap.Core.Modules.Standard.CrtSh
{
    /// <summary>
    /// Result produced by <see cref="CrtShModule"/>.
    /// </summary>
    public class CrtShModuleResult : AbstractModuleResult
    {
        public DomainKnowledge DomainKnowledge { get; set; } = null!;

        public override IReadOnlyList<AbstractKnowledge> Knowledge => [
            DomainKnowledge
        ];
    }
}
