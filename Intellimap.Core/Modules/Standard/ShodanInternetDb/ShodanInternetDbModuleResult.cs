using Intellimap.Core.Knowledge;
using Intellimap.Core.Knowledge.Network;

namespace Intellimap.Core.Modules.Standard.ShodanInternetDb
{
    /// <summary>
    /// Result produced by <see cref="ShodanInternetDbModule"/>.
    /// </summary>
    public class ShodanInternetDbModuleResult : AbstractModuleResult
    {
        public ExposureKnowledge ExposureKnowledge { get; set; } = null!;

        public override IReadOnlyList<AbstractKnowledge> Knowledge => [
            ExposureKnowledge
        ];
    }
}
