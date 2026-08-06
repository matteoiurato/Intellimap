namespace Intellimap.Core.Knowledge.Identity
{
    /// <summary>
    /// Knowledge describing a social/platform profile associated with a username.
    /// </summary>
    public class ProfileKnowledge : AbstractKnowledge
    {
        public string Username { get; set; } = string.Empty;

        public string? DisplayName { get; set; }

        public string? Bio { get; set; }

        public string? Company { get; set; }

        public string? Location { get; set; }

        public string? Website { get; set; }

        public string? AvatarUrl { get; set; }

        public byte[]? AvatarImage { get; set; }

        public string? AvatarImageContentType { get; set; }

        public DateTime? CreatedAt { get; set; }
    }
}
