using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ArkData
{
    /// <summary>
    /// Represents a player together with ARK and Steam information.
    /// </summary>
    /// <seealso cref="ArkData.IPlayer" />
    [DataContract]
    public class Player : IPlayer
    {
        [DataMember]
        public ulong Id { get; set; }

        [DataMember]
        public string SteamId { get; set; }

        [DataMember]
        public string SteamName { get; set; }

        [DataMember]
        public string CharacterName { get; set; }

        [DataMember]
        public ushort Level { get; set; }

        [DataMember]
        public float ExperiencePoints { get; set; }

        [DataMember]
        public int TotalEngramPoints { get; set; }

        [DataMember]
        public bool FirstSpawned { get; set; }

        [DataMember]
        public ISteamPlayerBanInfo BanInfo { get; set; }

        [DataMember]
        public ISteamPlayerInfo PlayerInfo { get; set; }

        [DataMember]
        public DateTime FileCreated { get; set; }

        [DataMember]
        public DateTime FileUpdated { get; set; }

        [DataMember]
        public bool Online { get; set; }

        [DataMember]
        public int TribeId { get; set; }

        [DataMember]
        public ITribe Tribe { get; set; }

        [DataMember]
        public IEnumerable<ITribe> OwnedTribes { get; set; }
    }
}
