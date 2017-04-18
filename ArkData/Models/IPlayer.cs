using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ArkData
{
    /// <summary>
    /// Represents a player together with ARK and Steam information.
    /// </summary>
    public interface IPlayer
    {
        ulong Id { get; set; }

        string SteamId { get; set; }

        string SteamName { get; set; }

        string CharacterName { get; set; }

        ushort Level { get; set; }

        float ExperiencePoints { get; set; }

        int TotalEngramPoints { get; set; }

        bool FirstSpawned { get; set; }

        ISteamPlayerBanInfo BanInfo { get; set; }

        ISteamPlayerInfo PlayerInfo { get; set; }

        DateTime FileCreated { get; set; }

        DateTime FileUpdated { get; set; }

        bool Online { get; set; }

        int TribeId { get; set; }

        ITribe Tribe { get; set; }

        IEnumerable<ITribe> OwnedTribes { get; set; }
    }
}