using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opdracht1
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
