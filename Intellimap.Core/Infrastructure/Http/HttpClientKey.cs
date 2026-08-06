namespace Intellimap.Core.Infrastructure.Http
{
    /// <summary>
    /// Key used by <see cref="HttpClientCache"/> to group requests into shared <see cref="HttpClient"/> instances.
    /// </summary>
    internal readonly record struct HttpClientKey(
        string? ProxyAddress
    );
}
