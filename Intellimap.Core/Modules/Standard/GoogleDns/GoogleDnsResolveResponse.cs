namespace Intellimap.Core.Modules.Standard.GoogleDns
{
    /// <summary>
    /// Response payload returned by the Google DNS-over-HTTPS resolve endpoint.
    /// </summary>
    internal class DnsResolveResponse
    {
        public int Status { get; set; }
        public List<DnsAnswer>? Answer { get; set; }
    }

    /// <summary>
    /// A single answer entry within a <see cref="DnsResolveResponse"/>.
    /// </summary>
    internal class DnsAnswer
    {
        public string Name { get; set; } = string.Empty;
        public int Type { get; set; }
        public int TTL { get; set; }
        public string Data { get; set; } = string.Empty;
    }
}
