using System;
using System.Collections.Generic;
using System.Linq;
using Opdracht1;

namespace Rogue.DomainObjects
{
    [Serializable]
    public class Zone
    {
        public Dungeon dungeon;
        public List<Node> nodes;
        public int number;
        public Node startNode;
        public Node endNode;

        public Zone(Dungeon dungeon, List<Node> nodes, Node startNode, Node endNode, int number)
        {
            this.dungeon = dungeon;

            this.nodes = nodes;
            foreach (Node node in nodes) {
                node.zone = this;
            }
            this.startNode = startNode;
            this.endNode = endNode;

            endNode.zone = this;
            this.number = number;
        }


        public bool isEndZone()
        {
            return this.Equals(this.dungeon.zones.Last());
        }
    }
}
