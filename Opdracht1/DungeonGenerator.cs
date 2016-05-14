using System;
using System.Linq;

namespace Opdracht1
{
    public class DungeonGenerator
    {
        private readonly ZoneGenerator zoneGenerator;

        public DungeonGenerator(ZoneGenerator zoneGenerator)
        {
            this.zoneGenerator = zoneGenerator;
        }

        public Dungeon generate(int level)
        {
            Dungeon dungeon = new Dungeon(level);

            int numZones = level + 1;
            Zone previousZone = null;
            for (int i = 0; i < numZones; i++)
            {
                previousZone = this.zoneGenerator.generate(previousZone?.endNode);
                dungeon.zones.Add(previousZone);
            }

            return dungeon;
        }

    }
}
