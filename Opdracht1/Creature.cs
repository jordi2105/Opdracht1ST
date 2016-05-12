using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opdracht1
{
    [Serializable]
    public abstract class Creature
    {
        int attackRating;
        public int hitPoints { get; set; }

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
