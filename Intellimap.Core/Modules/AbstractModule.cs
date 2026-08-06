using Intellimap.Core.Modules.Options;

namespace Intellimap.Core.Modules
{
    /// <summary>
    /// Common runtime base class for modules.
    /// Provides a non-generic type used by the module manager.
    /// Module-specific execution logic is implemented in <see cref="AbstractModule{TResult}"/>.
    /// </summary>
    public abstract class AbstractModule
    {
        /// <summary>
        /// The unique identifier of the module.
        /// </summary>
        public abstract string ModuleId { get; }

        /// <summary>
        /// The target types this module can be executed against.
        /// </summary>
        public abstract IReadOnlyCollection<Type> SupportedTargetTypes { get; }

        /// <summary>
        /// The knowledge types this module can produce.
        /// </summary>
        public abstract IReadOnlyCollection<Type> ProducedKnowledgeTypes { get; }
    }

    /// <summary>
    /// Generic base class for implementing modules.
    /// Defines the strongly typed execution contract between module execution context and module results.
    /// </summary>
    /// <typeparam name="TResult">
    /// The result type returned by the module.
    /// </typeparam>
    public abstract class AbstractModule<TResult> : AbstractModule
        where TResult : AbstractModuleResult, new()
    {
        /// <summary>
        /// Creates a new result instance.
        /// </summary>
        protected TResult CreateResult()
        {
            return new TResult
            {
                SourceModuleId = ModuleId
            };
        }

        /// <summary>
        /// Executes the module against the given context.
        /// </summary>
        public async Task<TResult> ExecuteAsync(ModuleExecutionContext context)
        {
            var httpOptions = context.Options as IHttpOptions;

            using var cts = CancellationTokenSource.CreateLinkedTokenSource(context.CancellationToken);
            if (httpOptions?.Timeout != null)
                cts.CancelAfter(httpOptions.Timeout.Value);

            return await ExecuteCoreAsync(context, cts.Token);
        }

        /// <summary>
        /// Implements the module's execution logic.
        /// </summary>
        protected abstract Task<TResult> ExecuteCoreAsync(ModuleExecutionContext context, CancellationToken cancellationToken);
    }
}
