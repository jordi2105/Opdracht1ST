using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Opdracht1;

namespace Opdracht1
{
    public class DungeonGenerator
    {
        private Random random;
        private List<Node> nodes;
        public int number;
        private int zoneCounter;

        public DungeonGenerator(Random random)
        {
            this.random = random;
            
        }

        public Dungeon generate(int level)
        {
            this.number = 0;
            this.zoneCounter = 0;
            Dungeon dungeon = new Dungeon(level);

            int numZones = level + 1;
            Zone[] zones = new Zone[numZones];
            Zone zone = null;
            for (int i = 0; i < numZones; i++)
            {
                zone = this.createNewZone(zone);
                this.removeDoubles(dungeon, zone);
                dungeon.zones.Add(zone);
            }

            return dungeon;
        }

        private void removeDoubles(Dungeon dungeon, Zone zone)
        {
            foreach (Node node in zone.nodes) {
                List<Node> newNeighbours = node.neighbours.Distinct().ToList();
                node.neighbours = newNeighbours;
            }
            if (zone.startNode.neighbours.Count == 1) {
                int index = this.random.Next(1, this.nodes.Count());
                Node node = this.nodes[index];
                while (node == zone.startNode) {
                    index++;
                    node = this.nodes[index];
                }

                zone.startNode.neighbours.Add(node);
            }
        }

        private Zone createNewZone(Zone previousZone)
        {
            this.nodes = new List<Node>();

            Node startNode = this.getStartingNode(previousZone, true);
            Node endNode = this.chooseEndNode(startNode);
            zoneCounter++;
            return new Zone(this.nodes, startNode, endNode, this.zoneCounter);
        }

        private Node getStartingNode(Zone previousZone, bool first)
        {
            if (previousZone != null)
                return this.createNodeTree(previousZone.endNode, first);

            return this.createNodeTree(first);
        }

        private Node createNodeTree(bool first)
        {
            Node node = new Node(this.number);

            return this.createNodeTree(node, first);
        }

        private Node createNodeTree(Node startNode, bool first)
        {
            this.number++;
            if(!first)
                this.nodes.Add(startNode);

            int num = this.getNumNeighbours();
            for (int i = 0; i < num; i++)
            {
                Node neighbour = this.getNeighbour(startNode);
                startNode.neighbours.Add(neighbour);
                neighbour.neighbours.Add(startNode);
            }

            return startNode;
        }

        private int getNumNeighbours()
        {
            if (this.nodes.Count > 5)
            {
                return 0;
            }

            return this.random.Next(
                this.getMinNeighbours(),
                this.getMaxNeighbours()
            );
        }

        private int getMinNeighbours()
        {
            if (this.nodes.Count > 2)
            {
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
            if (count < 3 | this.random.Next(2) == 0)
            {
                return this.createNodeTree(false);
            }

            List<Node> possibleNeighbours = this.findPossibleNeighbours(node);
            int numChoices = possibleNeighbours.Count;
            if (numChoices == 0)
            {
                return this.createNodeTree(false);
            }

            return possibleNeighbours[this.random.Next(numChoices)];
        }

        private List<Node> findPossibleNeighbours(Node node)
        {
            return this.nodes.FindAll(neighbour => this.isValidNeighbour(node, neighbour));
        }

        private bool isValidNeighbour(Node node, Node neighbour)
        {
            if (neighbour.Equals(node))
            {
                return false;
            }

            if (neighbour.neighbours.Count > 3)
            {
                return false;
            }

            if (node.neighbours.Contains(neighbour))
            {
                return false;
            }

            return true;
        }

        private Node chooseEndNode(Node startingNode)
        {
            Node node = this.nodes[this.random.Next(this.nodes.Count())];
            if (!node.Equals(startingNode) && node.neighbours.Count >= 2)
            {
                return node;
            }

            return this.chooseEndNode(startingNode);
        }

    }
}
