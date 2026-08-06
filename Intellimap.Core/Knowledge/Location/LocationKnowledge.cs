namespace Intellimap.Core.Knowledge.Location
{
    /// <summary>
    /// Knowledge describing a geographic location.
    /// </summary>
    public class LocationKnowledge : IKnowledge
    {
        public string? Country { get; set; }

        public string? CountryCode { get; set; }

        public string? Region { get; set; }

        public string? RegionName { get; set; }

        public string? City { get; set; }

        public string? PostalCode { get; set; }

        public double? Latitude { get; set; }

        public double? Longitude { get; set; }
    }
}
