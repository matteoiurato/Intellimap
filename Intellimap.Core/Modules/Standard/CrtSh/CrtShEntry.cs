using System.Text.Json.Serialization;

namespace Intellimap.Core.Modules.Standard.CrtSh
{
    /// <summary>
    /// A single certificate transparency log entry returned by the crt.sh JSON API.
    /// </summary>
    internal class CrtShEntry
    {
        [JsonPropertyName("common_name")]
        public string CommonName { get; set; } = string.Empty;

        [JsonPropertyName("name_value")]
        public string NameValue { get; set; } = string.Empty;

        [JsonPropertyName("issuer_name")]
        public string IssuerName { get; set; } = string.Empty;

        [JsonPropertyName("not_before")]
        public DateTime NotBefore { get; set; }

        [JsonPropertyName("not_after")]
        public DateTime NotAfter { get; set; }

        [JsonPropertyName("serial_number")]
        public string SerialNumber { get; set; } = string.Empty;
    }
}
