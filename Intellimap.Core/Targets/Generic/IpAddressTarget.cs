using System.Net;

namespace Intellimap.Core.Targets.Generic
{
    /// <summary>
    /// A target identifying a single IP address (e.g. for geolocation or network-owner lookups).
    /// </summary>
    public record IpAddressTarget : AbstractTarget
    {
        /// <summary>
        /// The parsed IP address.
        /// </summary>
        public IPAddress Address { get; }

        /// <summary>
        /// Creates a new target from an IP address string.
        /// </summary>
        /// <param name="address">A valid IPv4 or IPv6 address.</param>
        /// <exception cref="ArgumentException"><paramref name="address"/> is not a valid IP address.</exception>
        public IpAddressTarget(string address)
        {
            if (!IPAddress.TryParse(address, out var ip))
                throw new ArgumentException("Invalid IP address");

            Address = ip;
        }
    }
}
