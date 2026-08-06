using Intellimap.Core.Knowledge;

namespace Intellimap.Core.Modules
{
    /// <summary>
    /// Base class for the result produced by a module's execution.
    /// </summary>
    public abstract class AbstractModuleResult
    {
        public string SourceModuleId { get; set; } = null!;

        public DateTime RetrievedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// The knowledge produced by the module.
        /// </summary>
        public abstract IReadOnlyList<AbstractKnowledge> Knowledge { get; }
    }
}
