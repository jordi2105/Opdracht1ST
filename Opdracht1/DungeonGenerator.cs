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
        private List<Node> nodes;
    
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
            this.nodes = new List<Node>();

            Node startingNode = this.createNodeTree();
            Node endNode = this.chooseEndNode(startingNode);

            return new Zone(this.nodes, startingNode, endNode);
        }

        private Node createNodeTree()
        {
            Node node = new Node(this.nodes.Count);
            this.nodes.Add(node);

            int num = this.getNumNeighbours();
            for (int i = 0; i < num; i++) {
                Node neighbour = this.getNeighbour(node);
                node.addNeighbour(neighbour);
            }

            return node;
        }

        private int getNumNeighbours()
        {
            if (this.nodes.Count > 5) {
                return 0;
            }

            return this.random.Next(
                this.getMinNeighbours(), 
                this.getMaxNeighbours()
            );
        }

        private int getMinNeighbours()
        {
            if (this.nodes.Count > 2) {
                return 0;
            }

            return 1;
        }

        private int getMaxNeighbours()
        {
            return Math.Min(5, 11 - this.nodes.Count);
        }

        private Node getNeighbour(Node node)
        {
            int count = this.nodes.Count;
            if (count < 3 | this.random.Next(2) == 0) {
                return this.createNodeTree();
            }

            List<Node> possibleNeighbours = this.findPossibleNeighbours(node);
            int numChoices = possibleNeighbours.Count;
            if (numChoices == 0) {
                return this.createNodeTree();
            }

            return possibleNeighbours[this.random.Next(numChoices)];
        }

        private List<Node> findPossibleNeighbours(Node node)
        {
            return this.nodes.FindAll(neighbour => this.isValidNeighbour(node, neighbour));
        }

        private bool isValidNeighbour(Node node, Node neighbour)
        {
            if (neighbour.Equals(node)) {
                return false;
            }

            if (neighbour.numNeighbours() > 3) {
                return false;
            }

            if (node.hasNeighbour(neighbour)) {
                return false;
            }

            return true;
        }


        private Node chooseEndNode(Node startingNode)
        {
            Node node = this.nodes[this.random.Next(this.nodes.Count())];
            if (!node.Equals(startingNode)) {
                return node;
            }

            return this.chooseEndNode(startingNode);
        }

    }
}
