namespace Intellimap.Core.Modules.Standard.ShodanInternetDb
{
    /// <summary>
    /// Response payload returned by the Shodan InternetDB endpoint.
    /// </summary>
    internal class ShodanInternetDbResponse
    {
        public string Ip { get; set; } = string.Empty;

        public List<int> Ports { get; set; } = [];

        public List<string> Hostnames { get; set; } = [];

        public List<string> Cpes { get; set; } = [];

        public List<string> Tags { get; set; } = [];

        public List<string> Vulns { get; set; } = [];
    }
}
