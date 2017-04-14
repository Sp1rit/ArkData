using System;
using System.Collections.Generic;

namespace ArkData
{
    /// <summary>
    /// Represents a tribe in ARK.
    /// </summary>
    public interface ITribe
    {
        DateTime FileCreated { get; set; }

        DateTime FileUpdated { get; set; }

        int Id { get; set; }

        ICollection<IPlayer> Members { get; set; }

        string Name { get; set; }

        IPlayer Owner { get; set; }

        uint OwnerId { get; set; }
    }
}