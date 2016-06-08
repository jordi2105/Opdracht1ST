using System;
using System.Collections.Generic;
using Opdracht1;
using System.Linq;

namespace Rogue.DomainObjects
{

    public class MonsterSpawner
    {
        public const int M = 3;
        public int N = 6;
        public const int O = 20;

        private readonly Random random;

        public MonsterSpawner(Random random)
        {
            this.random = random;
        }

        public void spawn(Dungeon dungeon)
        {

            foreach (Zone zone in dungeon.zones)
            {
                spawnMonstersInZone(zone, dungeon, 0);
            }
        }

        public void spawnMonstersInZone(Zone zone, Dungeon dungeon, int numberOfMonstersPlaced)
        {
            int maxMonstersInNode = M * (dungeon.level + 1);
            int numberOfMonsters =
                (2 * zone.number * O) /
                ((dungeon.level + 2) * (dungeon.level + 1));
            int monstersLeft = numberOfMonsters - numberOfMonstersPlaced;
            List<Node> notFullNodes = new List<Node>(zone.nodes);
            while (monstersLeft > 0 && notFullNodes.Count > 0)
            {
                int index = this.random.Next(1, notFullNodes.Count);
                Node node = notFullNodes[index];
                int x = 0;
                foreach (Pack pack in node.packs)
                {
                    x += pack.monsters.Count;
                }
                if (node != zone.endNode)
                {
                    int count = this.random.Next(0, Math.Min(maxMonstersInNode - x, monstersLeft) + 1);
                    if (count != 0)
                    {
                        node.packs.Add(new Pack(count, node));
                        monstersLeft -= count;
                    }

                }

                x = 0;
                foreach (Pack pack in node.packs)
                {
                    x += pack.monsters.Count;
                }
                if (x == maxMonstersInNode)
                    notFullNodes.Remove(node);

            }
        }


        public void spawnAssignment5(Dungeon dungeon)
        {
            foreach(Zone zone in dungeon.zones)
            {
                int maxMonstersInNode = M * (dungeon.level + 1);
                int numberOfMonsters =
                    (2 * zone.number * O) /
                    ((dungeon.level + 2) * (dungeon.level + 1));
                int monstersLeft = numberOfMonsters;

                List<Node> neighbours = zone.startNode.neighbours;
                int counter = 0;
                foreach(Node neighbour in neighbours)
                {
                    if (neighbour.zone != null)
                    {
                        neighbour.packs.Add(new Pack(1, neighbour));
                        counter++;
                    }
                }
                this.spawnMonstersInZone(zone, dungeon, counter);
            }
            
        }

        public List<Node> getNodesWithShortestPath(Node startNode, Node endNode)
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
