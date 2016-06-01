using System;

namespace Rogue.DomainObjects
{
    [Serializable]
    public abstract class Item
    {
        public string getItemType()
        {
            if (this.GetType() == typeof(HealingPotion))
                return "HealingPotion";
            if (this.GetType() == typeof(TimeCrystal))
                return "TimeCrystal";
            return null;
        }
    }
}
