namespace Intellimap.Core.Exceptions
{
    /// <summary>
    /// Thrown when a module is executed against a target it does not support.
    /// </summary>
    public class InvalidModuleTargetException : Exception
    {
        /// <summary>
        /// The identifier of the module that rejected the target.
        /// </summary>
        public string ModuleId { get; }

        /// <summary>
        /// The unsupported target type.
        /// </summary>
        public Type TargetType { get; }

        public InvalidModuleTargetException(string moduleId, Type targetType)
            : base($"Module '{moduleId}' does not support target '{targetType.Name}'.")
        {
            ModuleId = moduleId;
            TargetType = targetType;
        }
    }
}
