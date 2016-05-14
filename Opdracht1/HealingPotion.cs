using System;

namespace Opdracht1
{
    [Serializable]
    public class HealingPotion : Item
    {
        public int hitPoints { get; }

        public HealingPotion(int hitPoints)
        {
            this.hitPoints = hitPoints;
        }
    }
}
