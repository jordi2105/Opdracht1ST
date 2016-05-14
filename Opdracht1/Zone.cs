using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opdracht1
{
    [Serializable]
    public class Zone
    {
        public List<Node> nodes;
        public int number;
        public Node startNode;
        public Node endNode;

        public Zone(List<Node> nodes, Node startNode, Node endNode, int number)
        {
            this.nodes = nodes;
            foreach (Node node in nodes)
                node.zone = this;
            this.startNode = startNode;
            this.endNode = endNode;
            endNode.zone = this;
            this.number = number;
        }
    }
}
