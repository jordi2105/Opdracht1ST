using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opdracht1
{
    class Zone
    {
        private Dungeon dungeon;
        private Node startNode;
        private Node endNode;

        public Zone(Dungeon dungeon, List<Node> nodes, Node startNode, Node endNode)
        {
            this.dungeon = dungeon;
            this.startNode = startNode;
            this.endNode = endNode;
        }
    }
}
