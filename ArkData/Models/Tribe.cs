using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ArkData
{
    /// <summary>
    /// Represents a tribe in ARK.
    /// </summary>
    /// <seealso cref="ArkData.ITribe" />
    [DataContract]
    public class Tribe : ITribe
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public uint OwnerId { get; set; }

        [DataMember]
        public DateTime FileCreated { get; set; }

        [DataMember]
        public DateTime FileUpdated { get; set; }

        [DataMember]
        public IPlayer Owner { get; set; }

        [DataMember]
        public ICollection<IPlayer> Members { get; set; }
    }
}
