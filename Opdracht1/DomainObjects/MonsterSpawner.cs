using System;
using System.Collections.Generic;

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
                int maxMonstersInNode = M * (dungeon.level + 1);
                int numberOfMonsters =
                    (2 * zone.number * O) /
                    ((dungeon.level + 2) * (dungeon.level + 1));
                int monstersLeft = numberOfMonsters;
                List<Node> notFullNodes = zone.nodes;
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
                        if(count != 0)
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
        }
    }
}
