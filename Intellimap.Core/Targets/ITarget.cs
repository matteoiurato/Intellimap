namespace Intellimap.Core.Targets
{
    /// <summary>
    /// Marker interface for anything a module can be run against. Modules declare which
    /// concrete target types they support via <see cref="Modules.AbstractModule.SupportedTargetTypes"/>.
    /// </summary>
    public interface ITarget
    {
    }
}
