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

        private List<Node> nodes;
        private Node startNode;
        private Node endNode;

        public Zone(List<Node> nodes, Node startNode, Node endNode)
        {
            this.nodes = nodes;
            this.startNode = startNode;
            this.endNode = endNode;
        }

        public List<Node> getNodes()
        {
            return nodes;
        }

        public Node getStartNode()
        {
            return startNode;
        }

        

            
    }
}
