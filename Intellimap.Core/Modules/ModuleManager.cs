using Intellimap.Core.Knowledge;
using Intellimap.Core.Modules.Loading;
using Intellimap.Core.Targets;
using System.Reflection;

namespace Intellimap.Core.Modules
{
    /// <summary>
    /// Central registry of module instances, built-in and external, discovered by scanning
    /// the default modules directory and any additional directories loaded at runtime.
    /// </summary>
    public static class ModuleManager
    {
        private const string DefaultModulesDirectoryName = "modules";

        private static readonly Dictionary<Type, Lazy<AbstractModule>> _modules = new();
        private static readonly HashSet<string> _loadedPluginPaths = new(StringComparer.OrdinalIgnoreCase);

        static ModuleManager()
        {
            LoadAssembliesFromDirectory(Path.Combine(AppContext.BaseDirectory, DefaultModulesDirectoryName));
            DiscoverModules();
        }

        /// <summary>
        /// Loads every assembly found under <paramref name="directory"/> (recursively) and
        /// registers any modules they contain, in addition to the default "modules" folder
        /// next to the executable. Safe to call multiple times.
        /// </summary>
        public static void LoadModulesFromDirectory(string directory)
        {
            LoadAssembliesFromDirectory(directory);
            DiscoverModules();
        }

        /// <summary>
        /// Loads every not-yet-loaded assembly found under <paramref name="directory"/>, each
        /// into its own <see cref="ExternalModuleLoadContext"/>.
        /// </summary>
        private static void LoadAssembliesFromDirectory(string directory)
        {
            if (!Directory.Exists(directory))
                return;

            foreach (var dllPath in Directory.GetFiles(directory, "*.dll", SearchOption.AllDirectories))
            {
                var fullPath = Path.GetFullPath(dllPath);
                if (!_loadedPluginPaths.Add(fullPath))
                    continue;

                try
                {
                    var context = new ExternalModuleLoadContext(fullPath);
                    context.LoadFromAssemblyPath(fullPath);
                }
                catch
                {
                    // A malformed or incompatible external module assembly shouldn't prevent
                    // every other module (built-in or external) from being discovered.
                }
            }
        }

        /// <summary>
        /// Scans all currently loaded assemblies and registers any newly found module types.
        /// </summary>
        private static void DiscoverModules()
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();

            foreach (var assembly in assemblies)
            {
                foreach (var type in GetLoadableTypes(assembly))
                {
                    if (!IsModule(type) || _modules.ContainsKey(type))
                        continue;

                    _modules[type] = new Lazy<AbstractModule>(() =>
                        (AbstractModule)Activator.CreateInstance(type)!);
                }
            }
        }

        /// <summary>
        /// Returns the types of an assembly, tolerating assemblies that fail to load some of their types.
        /// </summary>
        private static IEnumerable<Type> GetLoadableTypes(Assembly assembly)
        {
            try
            {
                return assembly.GetTypes();
            }
            catch (ReflectionTypeLoadException ex)
            {
                return ex.Types.Where(t => t != null)!;
            }
            catch
            {
                return [];
            }
        }

        /// <summary>
        /// Determines whether a type is a concrete, instantiable module.
        /// </summary>
        private static bool IsModule(Type type)
        {
            return type.IsClass
                && !type.IsAbstract
                && typeof(AbstractModule).IsAssignableFrom(type);
        }

        /// <summary>
        /// Gets the singleton instance of a module by its type.
        /// </summary>
        public static T GetModule<T>()
            where T : AbstractModule
        {
            if (!_modules.TryGetValue(typeof(T), out var module))
                throw new InvalidOperationException(
                    $"Module '{typeof(T).Name}' not found.");

            return (T)module.Value;
        }

        /// <summary>
        /// Gets every module that supports the given target type.
        /// </summary>
        public static IReadOnlyCollection<AbstractModule> GetModulesForTarget<TTarget>()
            where TTarget : AbstractTarget
        {
            return GetModulesForTarget(typeof(TTarget));
        }

        /// <summary>
        /// Gets every module that supports at least one of the given target types.
        /// </summary>
        public static IReadOnlyCollection<AbstractModule> GetModulesForTarget(params Type[] targetTypes)
        {
            return _modules.Values
                .Select(x => x.Value)
                .Where(x => x.SupportedTargetTypes.Intersect(targetTypes).Any())
                .ToArray();
        }

        /// <summary>
        /// Gets every module that can produce the given knowledge type.
        /// </summary>
        public static IReadOnlyCollection<AbstractModule> GetModulesProducingKnowledge<TKnowledge>()
            where TKnowledge : AbstractKnowledge
        {
            return GetModulesProducingKnowledge(typeof(TKnowledge));
        }

        /// <summary>
        /// Gets every module that can produce at least one of the given knowledge types.
        /// </summary>
        public static IReadOnlyCollection<AbstractModule> GetModulesProducingKnowledge(params Type[] knowledgeTypes)
        {
            return _modules.Values
                .Select(x => x.Value)
                .Where(x => x.ProducedKnowledgeTypes.Intersect(knowledgeTypes).Any())
                .ToArray();
        }
    }
}