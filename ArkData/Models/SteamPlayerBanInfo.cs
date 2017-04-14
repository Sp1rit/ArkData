using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ArkData
{
    [DataContract]
    public class SteamPlayerBanInfo : ISteamPlayerBanInfo
    {
        [DataMember]
        public string SteamId { get; set; }

        [DataMember]
        public bool CommunityBanned { get; set; }

        [DataMember]
        public bool VACBanned { get; set; }

        [DataMember]
        public int NumberOfVACBans { get; set; }

        [DataMember]
        public int DaysSinceLastBan { get; set; }

        [DataMember]
        public int NumberOfGameBans { get; set; }

        [DataMember]
        public string EconomyBan { get; set; }
    }

    [DataContract]
    internal class SteamPlayerBanInfoResponse
    {
        [DataMember(Name = "players")]
        public SteamPlayerBanInfo[] Players { get; set; }
    }
}
