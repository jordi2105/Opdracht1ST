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
        public int number = 0;
        private int M = 10, N = 6, O = 40;
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

            for (int i = 0; i < numZones; i++)
            {
                Zone zone = this.createNewZone(dungeon);

                this.removeDoubles(dungeon, zone);
                dungeon.addZone(zone);
            }

            

            return dungeon;
        }

        private Zone spawnMonsters(Dungeon dungeon, Zone zone)
        {
            int L = dungeon.getLevel();
            int maxMonstersInNode = M * (L + 1);
            int numberOfMonsters = (2 * zone.getZoneNumber() * O) / (L * (L + 1));
            int monstersLeft = numberOfMonsters;
            while (monstersLeft > 0)
            {
                int index = random.Next(1, nodes.Count());
                Node node = nodes[index];
                if (node != zone.getEndNode())
                {
                    int count = random.Next(1, maxMonstersInNode);
                    Pack pack = new Pack(count, node);
                    node.addPack(pack);
                    monstersLeft -= count;
                }


            }
            return zone;
        }

        private void removeDoubles(Dungeon dungeon, Zone zone)
        {
            foreach (Node node in zone.getNodes()) {
                List<Node> newNeighbours = node.getNeighbours().Distinct().ToList();
                node.setNeightbours(newNeighbours);
            }
            if (zone.getStartNode().getNeighbours().Count() == 1) {
                int index = this.random.Next(1, this.nodes.Count());
                Node node = this.nodes[index];
                while (node == zone.getStartNode()) {
                    index++;
                    node = this.nodes[index];
                }
                zone.getStartNode().addNeighbour(node);
                zone = spawnMonsters(dungeon, zone);
            }
        }

        private Zone createNewZone(Dungeon dungeon)
        {
            this.nodes = new List<Node>();

            Node startingNode = this.createNodeTree();
            Node endNode = this.chooseEndNode(startingNode);

            return new Zone(nodes, startingNode, endNode, zoneCounter);
        }

        private Node createNodeTree()
        {
            Node node = new Node(number);
            number++;
            this.nodes.Add(node);

            int num = this.getNumNeighbours();
            for (int i = 0; i < num; i++)
            {
                Node neighbour = this.getNeighbour(node);
                node.addNeighbour(neighbour);
            }

            return node;
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

            if (neighbour.numNeighbours() > 3)
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
            if (!node.Equals(startingNode) && node.numNeighbours() >= 2)
            {
                return node;
            }

            return this.chooseEndNode(startingNode);
        }

    }
}
