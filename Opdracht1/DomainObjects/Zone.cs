using System;
using System.Collections.Generic;
using Opdracht1;

namespace Rogue.DomainObjects
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
