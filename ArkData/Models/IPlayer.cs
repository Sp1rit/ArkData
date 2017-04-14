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
        ISteamPlayerBanInfo BanInfo { get; set; }

        string CharacterName { get; set; }

        DateTime FileCreated { get; set; }

        DateTime FileUpdated { get; set; }

        ulong Id { get; set; }

        ushort Level { get; set; }

        bool Online { get; set; }

        IEnumerable<ITribe> OwnedTribes { get; set; }

        ISteamPlayerInfo PlayerInfo { get; set; }

        string SteamId { get; set; }

        string SteamName { get; set; }

        ITribe Tribe { get; set; }

        int TribeId { get; set; }
    }
}