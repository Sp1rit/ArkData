using System;
using System.Collections.Generic;

namespace ArkData
{
    /// <summary>
    /// Represents a tribe in ARK.
    /// </summary>
    public interface ITribe
    {
        int Id { get; set; }

        string Name { get; set; }

        uint OwnerId { get; set; }

        DateTime FileCreated { get; set; }

        DateTime FileUpdated { get; set; }

        IPlayer Owner { get; set; }

        ICollection<IPlayer> Members { get; set; }
    }
}