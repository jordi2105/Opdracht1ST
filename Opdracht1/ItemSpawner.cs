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

            int monsterHitPoints = 0;
            foreach (Zone zone in zones) {
                foreach (Node node in zone.getNodes()) {
                    foreach (Pack pack in node.getPacks()) {
                        foreach (Monster monster in pack.Monsters) {
                            monsterHitPoints += monster.HitPoints;
                        }
                    }
                }
            }


        }
    }
}
