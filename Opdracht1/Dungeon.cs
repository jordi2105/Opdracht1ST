using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opdracht1
{
    [Serializable]
    class Dungeon
    {
        private List<Zone> zones;
        private List<Pack> packs;
        private List<Item> items;

        private int level;
        
        public Dungeon(int level)
        {
            this.zones = new List<Zone>();
            this.level = level;
        }

        private Node[] shortestpath(Node u, Node v)
        {
            return null;
        }

        public int getLevel()
        {
            return this.level;
        }

        public void addZone(Zone zone)
        {
            this.zones.Add(zone);
        }

        public List<Zone> getZones()
        {
            return this.zones;
        }
    }
}
