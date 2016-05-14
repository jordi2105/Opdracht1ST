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
        public int number = 0;
        private int zoneCounter = 1;

        public DungeonGenerator(Random random)
        {
            this.random = random;
        }

        public Dungeon generate(int level)
        {
            Dungeon dungeon = new Dungeon(level);

            int numZones = level + 1;
            Zone[] zones = new Zone[numZones];
            Zone zone = null;
            for (int i = 0; i < numZones; i++)
            {
                zone = this.createNewZone(zone);
                this.removeDoubles(zone);
                dungeon.zones.Add(zone);
            }

            return dungeon;
        }

        private void removeDoubles(Zone zone)
        {
            foreach (Node node in zone.nodes) {
                node.setNeighbours(node.getNeighbours().Distinct().ToList());
            }


            if (zone.startNode.neighbourCount() == 1) {
                List<Node> viableNeighnours = zone.nodes.FindAll(n => this.isValidNeighbour(zone.startNode, n));
                zone.startNode.addNeighbour(viableNeighnours[this.random.Next(viableNeighnours.Count)]);
            }
        }

        private Zone createNewZone(Zone previousZone)
        {
            this.nodes = new List<Node>();

            Node startNode = this.getStartingNode(previousZone);
            Node endNode = this.chooseEndNode(startNode);

            return new Zone(this.nodes, startNode, endNode, this.zoneCounter);
        }

        private Node getStartingNode(Zone previousZone)
        {
            if (previousZone != null)
                return this.createNodeTree(previousZone.endNode);

            return this.createNodeTree();
        }

        private Node createNodeTree()
        {
            Node node = new Node(this.number);

            return this.createNodeTree(node);
        }

        private Node createNodeTree(Node startNode)
        {
            this.number++;
            this.nodes.Add(startNode);

            int num = this.getNumNeighbours();
            for (int i = 0; i < num; i++)
            {
                Node neighbour = this.getNeighbour(startNode);
                startNode.addNeighbour(neighbour);
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
                return this.createNodeTree();
            }

            List<Node> possibleNeighbours = this.findPossibleNeighbours(node);
            int numChoices = possibleNeighbours.Count;
            if (numChoices == 0)
            {
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
            if (neighbour.Equals(node))
            {
                return false;
            }

            if (neighbour.neighbourCount() > 3)
            {
                return false;
            }

            if (node.hasNeighbour(neighbour))
            {
                return false;
            }

            return true;
        }

        private Node chooseEndNode(Node startingNode)
        {
            Node node = this.nodes[this.random.Next(this.nodes.Count())];
            if (!node.Equals(startingNode) && node.neighbourCount() >= 2)
            {
                return node;
            }

            return this.chooseEndNode(startingNode);
        }

    }
}
