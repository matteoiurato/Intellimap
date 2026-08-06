namespace Intellimap.Core.Knowledge.Network
{
    /// <summary>
    /// Knowledge describing the internet-facing exposure of an IP address.
    /// </summary>
    public class ExposureKnowledge : IKnowledge
    {
        public string IpAddress { get; set; } = string.Empty;

        public List<int> Ports { get; set; } = [];

        public List<string> Hostnames { get; set; } = [];

        public List<string> Cpes { get; set; } = [];

        public List<string> Tags { get; set; } = [];

        public List<string> Vulnerabilities { get; set; } = [];
    }
}
