using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opdracht1
{
    class Monster : Creature
    {
        public Pack pack;
        public Monster()
        {
            AttackRating = 5;
            HitPoints = 14;
        }
    }
}
