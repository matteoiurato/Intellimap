using Intellimap.Core.Modules.Options;
using Intellimap.Core.Targets;

namespace Intellimap.Core.Modules
{
    /// <summary>
    /// The set of parameters that make up a module's execution context.
    /// </summary>
    public class ModuleExecutionContext
    {
        public AbstractTarget Target { get; set; } = null!;

        public IModuleOptions? Options { get; set; }

        public CancellationToken CancellationToken { get; set; } = CancellationToken.None;
    }
}
