﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Opdracht1.Tests
{
    [TestClass()]
    public class MonsterSpawnerTests
    {
        [TestMethod]
        public void number_of_monsters_not_more_than_maximum()
        {
            Dungeon dungeon = this.createNewDungeon();

            foreach (Zone zone in dungeon.zones) {
                foreach (Node node in zone.nodes) {
                    int monsterCount = 0;
                    foreach (Pack pack in node.packs) {
                        monsterCount += pack.Monsters.Count;
                    }
                    Assert.IsTrue(monsterCount <= MonsterSpawner.M * (dungeon.level + 1));
                }
            }
        }

        

        [TestMethod]
        public void monster_are_divided_equally_over_zones()
        {
            Dungeon dungeon = this.createNewDungeon();

            foreach (Zone zone in dungeon.zones) {
                int monsterCount = 0;
                foreach (Node node in zone.nodes) {
                    foreach (Pack pack in node.packs) {
                        monsterCount += pack.Monsters.Count;
                    }
                }

                Assert.AreEqual(
                    monsterCount, 
                    (2*zone.number*MonsterSpawner.O) /
                    ((dungeon.level + 2)*(dungeon.level + 1))
                );

            }
        }


        private Dungeon createNewDungeon()
        {
            Random random = new Random();
            DungeonGenerator dungeonGenerator = new DungeonGenerator(new ZoneGenerator(random));
            MonsterSpawner monsterSpawner = new MonsterSpawner(random);

            Dungeon dungeon = dungeonGenerator.generate(3);
            monsterSpawner.spawn(dungeon);

            return dungeon;
        }
    }
}