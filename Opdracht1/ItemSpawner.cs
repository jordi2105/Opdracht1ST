using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opdracht1
{
    class ItemSpawner
    {
        private Random random;

        public ItemSpawner(Random random)
        {
            this.random = random;
        }

        public void spawn(Dungeon dungeon)
        {
            List<Zone> zones = dungeon.getZones();

            foreach (Zone zone in zones) {
                
            }
        }
    }
}
