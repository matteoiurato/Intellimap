using System.Text.Json.Serialization;

namespace Intellimap.Core.Modules.Standard.GitHub
{
    /// <summary>
    /// Response payload returned by the GitHub user profile endpoint.
    /// </summary>
    internal class GitHubUserResponse
    {
        public string Login { get; set; } = string.Empty;

        public string? Name { get; set; }

        public string? Bio { get; set; }

        public string? Company { get; set; }

        public string? Blog { get; set; }

        public string? Location { get; set; }

        [JsonPropertyName("avatar_url")]
        public string? AvatarUrl { get; set; }

        [JsonPropertyName("created_at")]
        public DateTime? CreatedAt { get; set; }
    }
}
