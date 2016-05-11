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
        private int zoneNumber;

        private List<Node> nodes;
        private Node startNode;
        private Node endNode;

        public Zone(List<Node> nodes, Node startNode, Node endNode, int zoneNumber)
        {
            this.nodes = nodes;
            this.startNode = startNode;
            this.endNode = endNode;
            this.zoneNumber = zoneNumber;
        }

        public int getZoneNumber()
        {
            return zoneNumber;
        }

        public List<Node> getNodes()
        {
            return nodes;
        }

        public Node getStartNode()
        {
            return startNode;
        }

        public Node getEndNode()
        {
            return endNode;
        }

        

            
    }
}
