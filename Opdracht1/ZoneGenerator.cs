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
            List<Node> nodes = new List<Node>();
            int numberOfNodes = this.random.Next(8, 15);
            for (int i = 0; i < numberOfNodes; i++) {
                nodes.Add(new Node(i));
            }

            Node startNode = nodes[0];
            Node endNode = nodes[nodes.Count - 1];

            int routeLength = this.random.Next(numberOfNodes/3, numberOfNodes/2);

            List<Node> connectedNodes = new List<Node>();

            this.moveNode(startNode, nodes, connectedNodes);
            this.moveNode(endNode, nodes, connectedNodes);

            Node previousNode = startNode;
            for (int i = 0; i < routeLength; i++) {
                Node nextNode = nodes[this.random.Next(nodes.Count)];
                this.moveNode(nextNode, nodes, connectedNodes);
                previousNode.addNeighbour(nextNode);
                previousNode = nextNode;
            }
            previousNode.addNeighbour(endNode);

            while (nodes.Count > 0) {
                Node node = nodes[this.random.Next(nodes.Count)];
                int numNeighbours = this.random.Next(1, 5);
                for (int i = 0; i < numNeighbours; i++) {
                    List<Node> neighbours = connectedNodes.FindAll(n => ! (n.neighbours.Count > 3));
                    Node neighbour = neighbours[this.random.Next(neighbours.Count)];
                    node.addNeighbour(neighbour);
                    this.moveNode(node, nodes, connectedNodes);
                }
            }

            Zone zone = new Zone(connectedNodes, startNode, endNode, this.zoneCounter++);

            return zone;
        }

        private void moveNode(Node node, List<Node> from, List<Node> to)
        {
            from.Remove(node);
            to.Add(node);
        }
    }
}
