using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ArkData
{
    /// <summary>
    /// Class to parse ARK player data and store the information.
    /// </summary>
    public class Container
    {
        private readonly IParser<IPlayer> playerParser;
        private readonly IParser<ITribe> tribeParser;
        private readonly ISteamApi steamApi;
        private readonly ISteamServer steamServer;
        private readonly List<IPlayer> players;
        private readonly List<ITribe> tribes;

        /// <summary>
        /// Gets the players.
        /// </summary>
        /// <value>
        /// The players.
        /// </value>
        public IEnumerable<IPlayer> Players => players;

        /// <summary>
        /// Gets the tribes.
        /// </summary>
        /// <value>
        /// The tribes.
        /// </value>
        public IEnumerable<ITribe> Tribes => tribes;

        /// <summary>
        /// Initializes a new instance of the <see cref="Container" /> class.
        /// </summary>
        /// <param name="playerParser">The player parser.</param>
        /// <param name="tribeParser">The tribe parser.</param>
        /// <param name="steamApi">The steam API.</param>
        /// <param name="steamServer">The steam server.</param>
        public Container(IParser<IPlayer> playerParser, IParser<ITribe> tribeParser, ISteamApi steamApi, ISteamServer steamServer)
        {
            this.playerParser = playerParser;
            this.tribeParser = tribeParser;
            this.steamApi = steamApi;
            this.steamServer = steamServer;

            players = new List<IPlayer>();
            tribes = new List<ITribe>();
        }

        /// <summary>
        /// Sets the players and tribes so they don't have to be loaded from a directory.
        /// </summary>
        /// <param name="players">The players.</param>
        /// <param name="tribes">The tribes.</param>
        /// <exception cref="System.ArgumentNullException">players
        /// or
        /// tribes</exception>
        public void SetData(IEnumerable<IPlayer> players, IEnumerable<ITribe> tribes)
        {
            if (players == null)
                throw new ArgumentNullException(nameof(players));
            if (tribes == null)
                throw new ArgumentNullException(nameof(tribes));

            this.players.Clear();
            this.players.AddRange(players);

            this.tribes.Clear();
            this.tribes.AddRange(tribes);
        }


        /// <summary>
        /// Loads the directory.
        /// </summary>
        /// <param name="directory">The directory.</param>
        /// <param name="linkPlayerTribes">if set to <c>true</c> [link player tribes].</param>
        /// <exception cref="System.IO.DirectoryNotFoundException">The provided directory doesn't exist.</exception>
        /// <exception cref="System.IO.FileLoadException"></exception>
        public void LoadDirectory(string directory, bool linkPlayerTribes = true)
        {
            if (!Directory.Exists(directory))
                throw new DirectoryNotFoundException("The provided directory doesn't exist.");

            var playerFiles = Directory.GetFiles(directory, "*.arkprofile");
            var tribeFiles = Directory.GetFiles(directory, "*.arktribe");

            if (playerFiles.Length == 0 && tribeFiles.Length == 0)
                throw new FileLoadException($"The directory does not contain any ark data files.");

            players.Clear();
            foreach (var file in playerFiles)
                players.Add(playerParser.Parse(file));

            tribes.Clear();
            foreach (var file in tribeFiles)
                tribes.Add(tribeParser.Parse(file));

            if (linkPlayerTribes)
                LinkPlayersAndTribes();
        }

        /// <summary>
        /// Loads the directory asynchronous.
        /// </summary>
        /// <param name="directory">The directory.</param>
        /// <param name="linkPlayerTribes">if set to <c>true</c> [link player tribes].</param>
        /// <returns></returns>
        /// <exception cref="System.IO.DirectoryNotFoundException">The provided directory doesn't exist.</exception>
        /// <exception cref="System.IO.FileLoadException"></exception>
        public async Task LoadDirectoryAsync(string directory, bool linkPlayerTribes = true)
        {
            if (!Directory.Exists(directory))
                throw new DirectoryNotFoundException("The provided directory doesn't exist.");

            var playerFiles = Directory.GetFiles(directory, "*.arkprofile");
            var tribeFiles = Directory.GetFiles(directory, "*.arktribe");

            if (playerFiles.Length == 0 && tribeFiles.Length == 0)
                throw new FileLoadException($"The directory does not contain any ark data files.");

            players.Clear();
            foreach (var file in playerFiles)
                players.Add(await playerParser.ParseAsync(file));

            tribes.Clear();
            foreach (var file in tribeFiles)
                tribes.Add(await tribeParser.ParseAsync(file));

            if (linkPlayerTribes)
                LinkPlayersAndTribes();
        }

        /// <summary>
        /// Links the players and tribes.
        /// </summary>
        public void LinkPlayersAndTribes()
        {
            for (var i = 0; i < players.Count; i++)
            {
                var player = players[i];
                player.OwnedTribes = Tribes.Where(t => t.OwnerId == player.Id).ToArray();
                player.Tribe = Tribes.SingleOrDefault(t => t.Id == player.TribeId);
            }

            for (var i = 0; i < tribes.Count; i++)
            {
                var tribe = tribes[i];
                tribe.Owner = Players.SingleOrDefault(p => p.Id == tribe.OwnerId);
                tribe.Members = Players.Where(p => p.TribeId == tribe.Id).ToArray();
            }
        }

        /// <summary>
        /// Loads the steam data.
        /// </summary>
        /// <param name="apiKey">The API key.</param>
        /// <exception cref="System.ArgumentNullException">apiKey</exception>
        /// <exception cref="System.InvalidOperationException">Container needs to be initialized with steamApi to fetch steam data.</exception>
        public void LoadSteamData(string apiKey)
        {
            if (string.IsNullOrEmpty(apiKey))
                throw new ArgumentNullException(nameof(apiKey));
            if (steamApi == null)
                throw new InvalidOperationException("Container needs to be initialized with steamApi to fetch steam data.");

            steamApi.ApiKey = apiKey;

            var steamIds = players.Select(p => p.SteamId);
            var playerInfo = steamApi.LoadPlayerInfo(steamIds, 100);
            var banInfo = steamApi.LoadPlayerBans(steamIds, 100);

            LinkSteamInformation(playerInfo, banInfo);
        }

        /// <summary>
        /// Loads the steam data asynchronous.
        /// </summary>
        /// <param name="apiKey">The API key.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">apiKey</exception>
        /// <exception cref="System.InvalidOperationException">Container needs to be initialized with steamApi to fetch steam data.</exception>
        public async Task LoadSteamDataAsync(string apiKey)
        {
            if (string.IsNullOrEmpty(apiKey))
                throw new ArgumentNullException(nameof(apiKey));
            if (steamApi == null)
                throw new InvalidOperationException("Container needs to be initialized with steamApi to fetch steam data.");

            steamApi.ApiKey = apiKey;

            var steamIds = players.Select(p => p.SteamId);
            var playerInfo = await steamApi.LoadPlayerInfoAsync(steamIds, 100);
            var banInfo = await steamApi.LoadPlayerBansAsync(steamIds, 100);

            LinkSteamInformation(playerInfo, banInfo);
        }

        private void LinkSteamInformation(IEnumerable<ISteamPlayerInfo> playerInfo, IEnumerable<ISteamPlayerBanInfo> banInfo)
        {
            foreach (var player in players)
            {
                player.PlayerInfo = playerInfo.FirstOrDefault(p => p.SteamId == player.SteamId);
                player.BanInfo = banInfo.FirstOrDefault(p => p.SteamId == player.SteamId);
            }
        }

        /// <summary>
        /// Loads the online players.
        /// </summary>
        /// <param name="server">The server.</param>
        /// <exception cref="System.InvalidOperationException">Container needs to be initialized with steamServer to fetch online players.</exception>
        public void LoadOnlinePlayers(IPEndPoint server)
        {
            if (steamServer == null)
                throw new InvalidOperationException("Container needs to be initialized with steamServer to fetch online players.");

            var groupedPlayers = players.GroupBy(p => p.SteamName.Length > 16 ? p.SteamName.Substring(0, 16) : p.SteamName);
            var names = steamServer.GetOnlinePlayerNames(server).Select(n => n.Length > 16 ? n.Substring(0, 16) : n).ToList();

            foreach (var group in groupedPlayers)
            {
                bool online = names.Contains(group.Key);
                foreach (var player in group)
                    player.Online = online;
            }
        }

        /// <summary>
        /// Loads the online players asynchronous.
        /// </summary>
        /// <param name="server">The server.</param>
        /// <returns></returns>
        /// <exception cref="System.InvalidOperationException">Container needs to be initialized with steamServer to fetch online players.</exception>
        public Task LoadOnlinePlayersAsync(IPEndPoint server)
        {
            if (steamServer == null)
                throw new InvalidOperationException("Container needs to be initialized with steamServer to fetch online players.");

            return Task.Run(() => LoadOnlinePlayers(server));
        }

        /// <summary>
        /// Creates a new container.
        /// </summary>
        /// <returns></returns>
        public static Container Create()
        {
            return new Container(new PlayerFileParser(), new TribeFileParser(), new SteamApi(), new SteamServer());
        }
    }
}
