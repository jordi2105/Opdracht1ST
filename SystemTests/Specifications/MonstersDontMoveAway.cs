using System.Collections.Generic;
using System.Linq;
using Opdracht1;
using Rogue;
using Rogue.DomainObjects;
using System;

namespace SystemTests.Specifications
{
    class MonstersDontMoveAway : ISpecification
    {
        private Dictionary<Pack, int> packsDictionary;
        Game game;

        public MonstersDontMoveAway()
        {
            this.packsDictionary = new Dictionary<Pack, int>();
        }

        public bool validate(Game game)
        {
            this.game = game;
            if (!this.packsDictionary.Any()) {
                this.initialize(game.gameState.dungeon.zones);
                return true;
            }

            foreach (KeyValuePair<Pack, int> pair in this.packsDictionary) {
                List<Node> nodesToPlayer = shortestPath(pair.Key.node, game.gameState.player.currentNode);
                List<Node> nodesToEndNode = shortestPath(pair.Key.node, pair.Key.node.zone.endNode);
                int count = Math.Min(nodesToPlayer.Count(), nodesToEndNode.Count());
                if (count > pair.Value)
                    return false;
            }

            return true;
        }

        private void initialize(List<Zone> zones)
        {
            foreach (Zone zone in zones) {
                foreach (Node node in zone.nodes) {
                    foreach (Pack pack in node.packs) {
                        List<Node> nodesToPlayer = shortestPath(pack.node, game.gameState.player.currentNode);
                        List<Node> nodesToEndNode = shortestPath(pack.node, zone.endNode);
                        int count = Math.Min(nodesToPlayer.Count(), nodesToEndNode.Count());
                        this.packsDictionary.Add(pack, count);
                    }
                }
            }
        }

        public List<Node> shortestPath(Node startNode, Node endNode)
        {
            Queue<List<Node>> queue = new Queue<List<Node>>();
            List<Node> nodeList = new List<Node>();
            nodeList.Add(startNode);
            queue.Enqueue(nodeList);
            while (queue.Count > 0)
            {
                List<Node> current = queue.Dequeue();
                if (current.Last() == endNode)
                {
                    return current;
                }
                List<Node> neighbours = current.Last().neighbours;
                foreach (Node neighbour in neighbours)
                {
                    List<Node> nodes = new List<Node>(current);
                    nodes.Add(neighbour);
                    queue.Enqueue(nodes);
                }
            }
            return null;
        }
    }
}
