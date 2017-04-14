using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ArkData
{
    [DataContract]
    public class SteamPlayerInfo : ISteamPlayerInfo
    {
        [DataMember(Name = "steamid")]
        public string SteamId { get; set; }

        [DataMember(Name = "personaname")]
        public string PersonaName { get; set; }

        [DataMember(Name = "profileurl")]
        public string ProfileUrl { get; set; }

        [DataMember(Name = "avatar")]
        public string Avatar { get; set; }

        [DataMember(Name = "avatarmedium")]
        public string AvatarMedium { get; set; }

        [DataMember(Name = "avatarfull")]
        public string AvatarFull { get; set; }
    }

    [DataContract]
    internal class SteamPlayerInfoResponsePlayers
    {
        [DataMember(Name = "players")]
        public SteamPlayerInfo[] Players { get; set; }
    }

    [DataContract]
    internal class SteamPlayerInfoResponse
    {
        [DataMember(Name = "response")]
        public SteamPlayerInfoResponsePlayers Response { get; set; }
    }
}
