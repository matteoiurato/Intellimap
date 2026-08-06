namespace Intellimap.Core.Exceptions
{
    /// <summary>
    /// Thrown when a module fails during execution.
    /// </summary>
    public class ModuleExecutionException : Exception
    {
        /// <summary>
        /// The identifier of the module that failed.
        /// </summary>
        public string ModuleId { get; }

        public ModuleExecutionException(string moduleId, string message, Exception? innerException = null)
            : base(message, innerException)
        {
            ModuleId = moduleId;
        }
    }
}
