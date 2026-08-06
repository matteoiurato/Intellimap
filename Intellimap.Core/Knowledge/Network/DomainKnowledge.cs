namespace Intellimap.Core.Knowledge.Network
{
    /// <summary>
    /// Knowledge describing domains found to be related to a hostname.
    /// </summary>
    public class DomainKnowledge : IKnowledge
    {
        public string Hostname { get; set; } = string.Empty;

        public List<string> RelatedDomains { get; set; } = [];
    }
}
