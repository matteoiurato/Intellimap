using Intellimap.Core.Modules.Options;
using System.Collections.Concurrent;
using System.Net;

namespace Intellimap.Core.Infrastructure.Http
{
    /// <summary>
    /// Caches and reuses <see cref="HttpClient"/> instances across module calls, keyed by
    /// their effective HTTP options, so modules avoid the socket-exhaustion issues that come
    /// from creating a new client per request.
    /// </summary>
    internal static class HttpClientCache
    {
        private static readonly HttpClient _defaultClient = new();
        private static readonly ConcurrentDictionary<HttpClientKey, HttpClient> _clients = new();

        /// <summary>
        /// Gets a client configured for the given options, creating and caching one if needed.
        /// </summary>
        public static HttpClient Get(IHttpOptions? options)
        {
            var key = BuildKey(options);

            return key == default ? _defaultClient : _clients.GetOrAdd(key, Build);
        }

        /// <summary>
        /// Builds the cache key for a set of options.
        /// </summary>
        private static HttpClientKey BuildKey(IHttpOptions? options)
        {
            if (options is null || string.IsNullOrWhiteSpace(options.ProxyAddress))
                return default;

            var normalizedProxyAddress = options.ProxyAddress.Trim().TrimEnd('/').ToLowerInvariant();
            return new HttpClientKey(normalizedProxyAddress);
        }

        /// <summary>
        /// Creates a new client configured for the given cache key.
        /// </summary>
        private static HttpClient Build(HttpClientKey key)
        {
            var handler = new HttpClientHandler();

            if (key.ProxyAddress is not null)
            {
                handler.Proxy = new WebProxy(key.ProxyAddress);
                handler.UseProxy = true;
            }

            return new HttpClient(handler);
        }
    }
}
