using System;
using System.Collections.Generic;
using System.Linq;

namespace Opdracht1
{
    public class ZoneGenerator
    {
        private readonly Random random;

        private int zoneCounter;

        public ZoneGenerator(Random random)
        {
            this.random = random;
            this.zoneCounter = 0;
        }

        public Zone generate(Node startingNode)
        {
            List<Node> nodes = this.generate();
            if (startingNode != null) {
                nodes.Insert(0, startingNode);
            }


            int routeLength = this.random.Next(nodes.Count/3, nodes.Count/2);

            Node previousNode = nodes[0];
            for (int i = 0; i < routeLength; i++) {
                Node nextNode = this.getUnconnectedNode(nodes);
                previousNode.addNeighbour(nextNode);
                previousNode = nextNode;
            }

            
            while (true) {
                Node unconnected = this.getUnconnectedNode(nodes);
                Node neighbour = this.getNeighbour(nodes);

                if (unconnected == null || neighbour == null) break;

                unconnected.addNeighbour(neighbour);

            }

            this.printConnections(nodes);

            return new Zone(nodes, nodes[0], previousNode, this.zoneCounter++);
        }

        private List<Node> generate()
        {
            List<Node> nodes = new List<Node>();
            int numberOfNodes = this.random.Next(8, 15);
            for (int i = 0; i < numberOfNodes; i++) {
                nodes.Add(new Node(i));
            }
            return nodes;
        }

        private void printConnections(List<Node> nodes)
        {
            foreach (Node node in nodes) {
                Console.WriteLine('\n' + node.number.ToString() + ':');
                foreach (Node neighbour in node.neighbours) {
                    Console.WriteLine("  " + neighbour.number.ToString());
                }
            }
        }

        private Node getNeighbour(List<Node> nodes)
        {
            List<Node> viableNeighbours = nodes.FindAll(n => 
                n.neighbours.Count < 4 &&
                n.neighbours.Count > 0);

            return this.getRandomNode(viableNeighbours);
        }

        private Node getUnconnectedNode(List<Node> nodes)
        {
            List<Node> unconnectedNodes = nodes.FindAll(n => 
                n.neighbours.Count == 0 &&
                n.number != 0);

            return this.getRandomNode(unconnectedNodes);
        }

        private Node getRandomNode(List<Node> neighbours)
        {
            return neighbours.Count == 0 ? null : neighbours[this.random.Next(neighbours.Count)];
        }
    }
}
