﻿using System;

namespace Opdracht1
{

    public class MonsterSpawner
    {
        public const int M = 3;
        public const int N = 6;
        public const int O = 40;

        private readonly Random random;

        public MonsterSpawner(Random random)
        {
            this.random = random;
        }

        public void spawn(Dungeon dungeon)
        {

            foreach (Zone zone in dungeon.zones) {
                int maxMonstersInNode = M * (dungeon.level + 1);
                int numberOfMonsters = 
                    (2 * zone.number * O) / 
                    ((dungeon.level + 2) * (dungeon.level + 1));
                int monstersLeft = numberOfMonsters;
                while (monstersLeft > 0)
                {
                    int index = this.random.Next(1, zone.nodes.Count);
                    Node node = zone.nodes[index];
                    if (node != zone.endNode)
                    {
                        int count = this.random.Next(1, Math.Min(maxMonstersInNode, monstersLeft) + 1);
                        node.packs.Add(new Pack(count, node));
                        monstersLeft -= count;
                    }
                }
            }
        }
    }
}
