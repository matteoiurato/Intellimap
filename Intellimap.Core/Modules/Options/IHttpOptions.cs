namespace Intellimap.Core.Modules.Options
{
    /// <summary>
    /// Options for modules that make HTTP calls.
    /// </summary>
    public interface IHttpOptions
    {
        /// <summary>
        /// The proxy address to route requests through.
        /// </summary>
        public string? ProxyAddress => String.Empty;

        /// <summary>
        /// The request timeout. Null means no timeout.
        /// </summary>
        public TimeSpan? Timeout => null;
    }
}
