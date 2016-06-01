using System;

namespace Rogue.DomainObjects
{
    [Serializable]
    public class Monster : Creature
    {
        public Pack pack;
        public Monster(Pack pack)
        {
            this.pack = pack;
            this.attackRating = 5;
            this.hitPoints = 14;
        }
    }
}
