namespace Intellimap.Core.Knowledge
{
    /// <summary>
    /// Base type for a single piece of intelligence produced by a module. Exposed via
    /// <see cref="Modules.AbstractModuleResult.Knowledge"/> and declared per-module via
    /// <see cref="Modules.AbstractModule.ProducedKnowledgeTypes"/>.
    /// </summary>
    public abstract class AbstractKnowledge
    {
        /// <summary>
        /// Whether the module found matching data for the target. False means the call succeeded
        /// but the source reported nothing for it, as opposed to a failed call, which throws instead.
        /// </summary>
        public bool Found { get; set; } = true;
    }
}
