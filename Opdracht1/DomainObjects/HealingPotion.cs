using System;

namespace Rogue.DomainObjects
{
    [Serializable]
    public class HealingPotion : Item
    {
        public int hitPoints;

        public HealingPotion(int hitPoints)
        {
            this.hitPoints = hitPoints;
        }
    }
}
