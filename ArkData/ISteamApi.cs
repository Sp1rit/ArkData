using System.Collections.Generic;
using System.Threading.Tasks;

namespace ArkData
{
    /// <summary>
    /// Interface for communication with the steam api.
    /// </summary>
    public interface ISteamApi
    {
        /// <summary>
        /// Gets or sets the API key.
        /// </summary>
        /// <value>
        /// The API key.
        /// </value>
        string ApiKey { get; set; }

        /// <summary>
        /// Loads the player bans.
        /// </summary>
        /// <param name="steamIds">The steam ids.</param>
        /// <returns></returns>
        IEnumerable<ISteamPlayerBanInfo> LoadPlayerBans(IEnumerable<string> steamIds);

        /// <summary>
        /// Loads the player bans.
        /// </summary>
        /// <param name="steamIds">The steam ids.</param>
        /// <param name="chunkSize">Size of the chunk.</param>
        /// <returns></returns>
        IEnumerable<ISteamPlayerBanInfo> LoadPlayerBans(IEnumerable<string> steamIds, long chunkSize);

        /// <summary>
        /// Loads the player bans asynchronously.
        /// </summary>
        /// <param name="steamIds">The steam ids.</param>
        /// <returns></returns>
        Task<IEnumerable<ISteamPlayerBanInfo>> LoadPlayerBansAsync(IEnumerable<string> steamIds);

        /// <summary>
        /// Loads the player bans asynchronous.
        /// </summary>
        /// <param name="steamIds">The steam ids.</param>
        /// <param name="chunkSize">Size of the chunk.</param>
        /// <returns></returns>
        Task<IEnumerable<ISteamPlayerBanInfo>> LoadPlayerBansAsync(IEnumerable<string> steamIds, long chunkSize);

        /// <summary>
        /// Loads the player information.
        /// </summary>
        /// <param name="steamIds">The steam ids.</param>
        /// <returns></returns>
        IEnumerable<ISteamPlayerInfo> LoadPlayerInfo(IEnumerable<string> steamIds);

        /// <summary>
        /// Loads the player information.
        /// </summary>
        /// <param name="steamIds">The steam ids.</param>
        /// <param name="chunkSize">Size of the chunk.</param>
        /// <returns></returns>
        IEnumerable<ISteamPlayerInfo> LoadPlayerInfo(IEnumerable<string> steamIds, long chunkSize);

        /// <summary>
        /// Loads the player information asynchronous.
        /// </summary>
        /// <param name="steamIds">The steam ids.</param>
        /// <returns></returns>
        Task<IEnumerable<ISteamPlayerInfo>> LoadPlayerInfoAsync(IEnumerable<string> steamIds);

        /// <summary>
        /// Loads the player information asynchronous.
        /// </summary>
        /// <param name="steamIds">The steam ids.</param>
        /// <param name="chunkSize">Size of the chunk.</param>
        /// <returns></returns>
        Task<IEnumerable<ISteamPlayerInfo>> LoadPlayerInfoAsync(IEnumerable<string> steamIds, long chunkSize);
    }
}