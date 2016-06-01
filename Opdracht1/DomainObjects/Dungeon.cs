using System;
using System.Collections.Generic;

namespace Rogue.DomainObjects
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
    }
}
