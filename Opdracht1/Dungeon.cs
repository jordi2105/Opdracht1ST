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
        public List<Zone> zones { get; }
        public int level { get; }
        
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

        
    }
}
