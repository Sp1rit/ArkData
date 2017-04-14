using System.Collections.Generic;
using System.Net;

namespace ArkData
{
    /// <summary>
    /// Interface to retrieve online players from a steam server.
    /// </summary>
    public interface ISteamServer
    {
        /// <summary>
        /// Gets the online player names.
        /// </summary>
        /// <param name="server">The server.</param>
        /// <returns></returns>
        IEnumerable<string> GetOnlinePlayerNames(IPEndPoint server);
    }
}