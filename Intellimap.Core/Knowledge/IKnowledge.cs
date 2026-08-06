namespace Intellimap.Core.Knowledge
{
    /// <summary>
    /// Marker interface for a single piece of intelligence produced by a module. Exposed via
    /// <see cref="Modules.AbstractModuleResult.Knowledge"/> and declared per-module via
    /// <see cref="Modules.AbstractModule.ProducedKnowledgeTypes"/>.
    /// </summary>
    public interface IKnowledge
    {
    }
}
