using System;

namespace ArkData
{
    public interface ISteamPlayerBanInfo
    {
        bool CommunityBanned { get; set; }

        int DaysSinceLastBan { get; set; }

        string EconomyBan { get; set; }

        int NumberOfGameBans { get; set; }

        int NumberOfVACBans { get; set; }

        string SteamId { get; set; }

        bool VACBanned { get; set; }
    }
}