using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opdracht1
{
    abstract class Creature
    {
        int attackRating, hitPoints;
        Node currentLocation;
        public abstract bool hpBelowZero;
        public int HitPoints
        {
            get
            {
                return this.hitPoints;
            }
            set
            {
                this.hitPoints = value;
            }
        }

        public int AttackRating
        {
            get
            {
                return this.attackRating;
            }
            set
            {
                this.attackRating = value;
            }
        }

    }
}
