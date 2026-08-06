namespace Intellimap.Core.Knowledge.Network
{
    /// <summary>
    /// Knowledge describing DNS records.
    /// </summary>
    public class DnsKnowledge : AbstractKnowledge
    {
        public string Hostname { get; set; } = string.Empty;

        public List<string> ARecords { get; set; } = [];
        public List<string> AaaaRecords { get; set; } = [];
        public List<string> CnameRecords { get; set; } = [];
        public List<string> NsRecords { get; set; } = [];
        public List<string> TxtRecords { get; set; } = [];
        public List<MxRecord> MxRecords { get; set; } = [];
    }

    /// <summary>
    /// A single DNS MX record.
    /// </summary>
    public record MxRecord(string Exchange, int Priority);
}
