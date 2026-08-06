using System.Reflection;
using System.Runtime.Loader;

namespace Intellimap.Core.Modules.Loading
{
    /// <summary>
    /// Isolated load context for a single external module assembly. Resolves the module's own
    /// private dependencies from its output folder, but defers resolution of host-owned
    /// contract assemblies (e.g. Core) to the default context so that types like
    /// AbstractModule stay identical between host and module instead of becoming two
    /// distinct types with the same name.
    /// </summary>
    internal sealed class ExternalModuleLoadContext : AssemblyLoadContext
    {
        private static readonly HashSet<string> HostOwnedAssemblyNames = new(StringComparer.OrdinalIgnoreCase)
        {
            "Core"
        };

        private readonly AssemblyDependencyResolver _resolver;

        /// <summary>
        /// Creates a load context for the module assembly at <paramref name="modulePath"/>.
        /// </summary>
        public ExternalModuleLoadContext(string modulePath) : base(isCollectible: false)
        {
            _resolver = new AssemblyDependencyResolver(modulePath);
        }

        /// <summary>
        /// Resolves a dependency of the module assembly, deferring host-owned assemblies to the default context.
        /// </summary>
        protected override Assembly? Load(AssemblyName assemblyName)
        {
            if (assemblyName.Name != null && HostOwnedAssemblyNames.Contains(assemblyName.Name))
                return null;

            var path = _resolver.ResolveAssemblyToPath(assemblyName);
            return path != null ? LoadFromAssemblyPath(path) : null;
        }
    }
}
