using System;

namespace ArkData
{
    public interface ISteamPlayerInfo
    {
        string Avatar { get; set; }

        string AvatarFull { get; set; }

        string AvatarMedium { get; set; }

        string PersonaName { get; set; }

        string ProfileUrl { get; set; }

        string SteamId { get; set; }
    }
}