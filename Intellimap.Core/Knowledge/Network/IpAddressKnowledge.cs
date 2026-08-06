using Intellimap.Core.Knowledge.Location;

namespace Intellimap.Core.Knowledge.Network
{
    /// <summary>
    /// Knowledge describing an IP address.
    /// </summary>
    public class IpAddressKnowledge : AbstractKnowledge
    {
        public string IpAddress { get; set; } = string.Empty;

        public LocationKnowledge Location { get; set; } = new();

        public NetworkOrganizationKnowledge NetworkOrganization { get; set; } = new();

        public string? Timezone { get; set; }
    }
}
