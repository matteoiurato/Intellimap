namespace Intellimap.Core.Knowledge.Network
{
    /// <summary>
    /// Knowledge describing the network owner of an IP address.
    /// </summary>
    public class NetworkOrganizationKnowledge : AbstractKnowledge
    {
        public string? Name { get; set; }

        public string? Provider { get; set; }

        public string? AutonomousSystem { get; set; }
    }
}
