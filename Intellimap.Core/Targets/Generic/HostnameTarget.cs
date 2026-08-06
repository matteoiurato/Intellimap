namespace Intellimap.Core.Targets.Generic
{
    /// <summary>
    /// A target identifying a single hostname or domain (e.g. for DNS lookups).
    /// </summary>
    public class HostnameTarget : AbstractTarget
    {
        /// <summary>
        /// The hostname or domain name.
        /// </summary>
        public string Hostname { get; }

        /// <summary>
        /// Creates a new target from a hostname string.
        /// </summary>
        /// <param name="hostname">A non-empty hostname or domain name.</param>
        /// <exception cref="ArgumentException"><paramref name="hostname"/> is null or empty.</exception>
        public HostnameTarget(string hostname)
        {
            if (string.IsNullOrEmpty(hostname))
                throw new ArgumentException("Invalid hostname");

            Hostname = hostname;
        }
    }
}
