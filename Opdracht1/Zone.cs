using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opdracht1
{
    [Serializable]
    class Zone
    {
        public List<Node> nodes { get; }
        public int number { get; }
        public Node startNode { get; }
        public  Node endNode { get; }

        public Zone(List<Node> nodes, Node startNode, Node endNode, int number)
        {
            this.nodes = nodes;
            this.startNode = startNode;
            this.endNode = endNode;
            this.number = number;
        }
    }
}
