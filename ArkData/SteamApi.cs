using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace ArkData
{
    /// <summary>
    /// Class for communication with the steam api.
    /// </summary>
    /// <seealso cref="ArkData.ISteamApi" />
    public class SteamApi : ISteamApi
    {
        private const string baseUrl = "https://api.steampowered.com/";
        private const string infoUrl = "ISteamUser/GetPlayerSummaries/v0002/?key={0}&steamids={1}";
        private const string banUrl = "ISteamUser/GetPlayerBans/v1/?key={0}&steamids={1}";

        private readonly HttpClient client;

        /// <summary>
        /// Gets or sets the API key.
        /// </summary>
        /// <value>
        /// The API key.
        /// </value>
        public string ApiKey { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SteamApi"/> class.
        /// </summary>
        public SteamApi()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri(baseUrl);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        /// <summary>
        /// Loads the player information.
        /// </summary>
        /// <param name="steamIds">The steam ids.</param>
        /// <returns></returns>
        public IEnumerable<ISteamPlayerInfo> LoadPlayerInfo(IEnumerable<string> steamIds)
        {
            string result = GetString(infoUrl, steamIds);

            return Deserialize<SteamPlayerInfoResponse>(result).Response.Players;
        }

        /// <summary>
        /// Loads the player information.
        /// </summary>
        /// <param name="steamIds">The steam ids.</param>
        /// <param name="chunkSize">Size of the chunk.</param>
        /// <returns></returns>
        public IEnumerable<ISteamPlayerInfo> LoadPlayerInfo(IEnumerable<string> steamIds, long chunkSize)
        {
            return steamIds.Split(chunkSize).SelectMany(g => LoadPlayerInfo(g)).ToArray();
        }

        /// <summary>
        /// Loads the player information asynchronous.
        /// </summary>
        /// <param name="steamIds">The steam ids.</param>
        /// <returns></returns>
        public async Task<IEnumerable<ISteamPlayerInfo>> LoadPlayerInfoAsync(IEnumerable<string> steamIds)
        {
            string result = await GetStringAsync(infoUrl, steamIds);

            return Deserialize<SteamPlayerInfoResponse>(result).Response.Players;
        }

        /// <summary>
        /// Loads the player information asynchronous.
        /// </summary>
        /// <param name="steamIds">The steam ids.</param>
        /// <param name="chunkSize">Size of the chunk.</param>
        /// <returns></returns>
        public async Task<IEnumerable<ISteamPlayerInfo>> LoadPlayerInfoAsync(IEnumerable<string> steamIds, long chunkSize)
        {
            return await steamIds.Split(chunkSize).SelectManyAsync(g => LoadPlayerInfoAsync(g));
        }

        /// <summary>
        /// Loads the player bans.
        /// </summary>
        /// <param name="steamIds">The steam ids.</param>
        /// <returns></returns>
        public IEnumerable<ISteamPlayerBanInfo> LoadPlayerBans(IEnumerable<string> steamIds)
        {
            string result = GetString(banUrl, steamIds);

            return Deserialize<SteamPlayerBanInfoResponse>(result).Players;
        }

        /// <summary>
        /// Loads the player bans.
        /// </summary>
        /// <param name="steamIds">The steam ids.</param>
        /// <param name="chunkSize">Size of the chunk.</param>
        /// <returns></returns>
        public IEnumerable<ISteamPlayerBanInfo> LoadPlayerBans(IEnumerable<string> steamIds, long chunkSize)
        {
            return steamIds.Split(chunkSize).SelectMany(g => LoadPlayerBans(g)).ToArray();
        }

        /// <summary>
        /// Loads the player bans asynchronously.
        /// </summary>
        /// <param name="steamIds">The steam ids.</param>
        /// <returns></returns>
        public async Task<IEnumerable<ISteamPlayerBanInfo>> LoadPlayerBansAsync(IEnumerable<string> steamIds)
        {
            string result = await GetStringAsync(banUrl, steamIds);

            return Deserialize<SteamPlayerBanInfoResponse>(result).Players;
        }

        /// <summary>
        /// Loads the player bans asynchronous.
        /// </summary>
        /// <param name="steamIds">The steam ids.</param>
        /// <param name="chunkSize">Size of the chunk.</param>
        /// <returns></returns>
        public async Task<IEnumerable<ISteamPlayerBanInfo>> LoadPlayerBansAsync(IEnumerable<string> steamIds, long chunkSize)
        {
            return await steamIds.Split(chunkSize).SelectManyAsync(g => LoadPlayerBansAsync(g));
        }

        private string GetString(string url, IEnumerable<string> steamIds)
        {
            url = string.Format(url, ApiKey, string.Join(",", steamIds));

            var response = client.GetAsync(url).Result;
            response.EnsureSuccessStatusCode();

            return response.Content.ReadAsStringAsync().Result;
        }

        private async Task<string> GetStringAsync(string url, IEnumerable<string> steamIds)
        {
            url = string.Format(url, ApiKey, string.Join(",", steamIds));

            var response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }

        private TResponse Deserialize<TResponse>(string result)
        {
            var serializer = new DataContractJsonSerializer(typeof(TResponse));

            TResponse response;
            byte[] resultData = Encoding.UTF8.GetBytes(result);
            using (var ms = new MemoryStream(resultData))
                response = (TResponse)serializer.ReadObject(ms);

            return response;
        }
    }
}
