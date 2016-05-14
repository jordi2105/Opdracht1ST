using System;
using System.Collections.Generic;

namespace Opdracht1
{
    [Serializable]
    public class Dungeon
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
    }
}
