using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
