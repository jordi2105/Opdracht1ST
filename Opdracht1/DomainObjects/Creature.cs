using System;

namespace Rogue.DomainObjects
{
    [Serializable]
    public abstract class Creature
    {
        public int hitPoints { get; set; }
        public int attackRating { get; set; }

    }
}
