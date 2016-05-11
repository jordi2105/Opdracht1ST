using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opdracht1
{
    [Serializable]
    class HealingPotion : Item
    {
        public int hitPoints { get; }

        public HealingPotion(int hitPoints)
        {
            this.hitPoints = hitPoints;
        }
    }
}
