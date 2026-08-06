namespace Intellimap.Core.Targets.Generic
{
    /// <summary>
    /// A target identifying a single username, platform-agnostic.
    /// </summary>
    public record UsernameTarget : AbstractTarget
    {
        /// <summary>
        /// The username.
        /// </summary>
        public string Username { get; }

        /// <summary>
        /// Creates a new target from a username string.
        /// </summary>
        /// <param name="username">A non-empty username.</param>
        /// <exception cref="ArgumentException"><paramref name="username"/> is null or empty.</exception>
        public UsernameTarget(string username)
        {
            if (string.IsNullOrEmpty(username))
                throw new ArgumentException("Invalid username");

            Username = username;
        }
    }
}
