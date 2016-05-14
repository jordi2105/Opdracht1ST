using System;

namespace Opdracht1
{
    [Serializable]
    public class Monster : Creature
    {
        public Pack pack;
        public Monster(Pack pack)
        {
            this.pack = pack;
            this.AttackRating = 5;
            this.hitPoints = 14;
        }
    }
}
