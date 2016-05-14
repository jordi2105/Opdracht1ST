using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opdracht1
{
    [Serializable]
    public class Dungeon
    {
        public List<Zone> zones;
        public int level;
        
        public Dungeon(int level)
        {
            this.zones = new List<Zone>();
            this.level = level;
        }

        private Node[] shortestpath(Node u, Node v)
        {
            return null;
        }
    }
}
