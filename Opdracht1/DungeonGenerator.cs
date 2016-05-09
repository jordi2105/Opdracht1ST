using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Opdracht1;

namespace Opdracht1
{
    class DungeonGenerator
    {
        private Random random;
    
        public DungeonGenerator(Random random)
        {
            this.random = random;
        }

        public Dungeon generate(int level)
        {
            Dungeon dungeon = new Dungeon(level);
            int numZones = level + 1;
            Zone[] zones = new Zone[numZones];

            for (int i = 0; i < numZones; i++) {
                dungeon.addZone(this.createNewZone(dungeon));
            }

            return dungeon;
        }

        private Zone createNewZone(Dungeon dungeon)
        {
            List<Node> nodes = new List<Node>();

            Node startingNode = this.createNodeTree(nodes);
            Node endNode = this.chooseEndNode(nodes, startingNode);

            return new Zone(nodes, startingNode, endNode);
        }

        private Node chooseEndNode(List<Node> nodes, Node startingNode)
        {
            Node node = nodes[this.random.Next(nodes.Count())];
            if (!node.Equals(startingNode)) {
                return node;
            }

            return this.chooseEndNode(nodes, startingNode);
        }

        private Node createNodeTree(List<Node> nodes)
        {
            Node node = new Node();
            nodes.Add(node);

            if (nodes.Count > 10) {
                return node;
            }

            int numNeighbours = this.random.Next(0, 5);
            for (int j = 0; j < numNeighbours; j++) {
                node.addNeighbour(this.getNeighbour(node, nodes));
            }

            return node;
        }

        private Node getNeighbour(Node node, List<Node> nodes)
        {
            int count = nodes.Count;
            if (count < 2) {
                return this.createNodeTree(nodes);
            }

            int coin = this.random.Next(2);
            if (coin == 0) {
                return this.createNodeTree(nodes);
            }
            
            Node neighbour = nodes[this.random.Next(count)];
            if (neighbour == node) {
                return this.createNodeTree(nodes);
            }

            if (neighbour.NumNeighbours() > 3) {
                return this.createNodeTree(nodes);
            }

            return neighbour;
        }
    }
}
