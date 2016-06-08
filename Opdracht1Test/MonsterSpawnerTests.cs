using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Opdracht1;
using Rogue.DomainObjects;
using Rogue.Services;

namespace Opdracht1Test
{
    [TestClass()]
    public class MonsterSpawnerTests
    {
        [TestMethod]
        public void number_of_monsters_not_more_than_maximum()
        {
            for(int i = 0; i < 1000; i++)
            {
                Dungeon dungeon = this.createNewDungeon();

                foreach (Zone zone in dungeon.zones)
                {
                    foreach (Node node in zone.nodes)
                    {
                        int monsterCount = 0;
                        foreach (Pack pack in node.packs)
                        {
                            monsterCount += pack.monsters.Count;
                        }
                        Assert.IsTrue(monsterCount <= 3 * (dungeon.level + 1));
                    }
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
                        monsterCount += pack.monsters.Count;
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
            IInputReader playerInputReader = new PlayerInputReader();
            DungeonGenerator dungeonGenerator = new DungeonGenerator(random, playerInputReader);
            MonsterSpawner monsterSpawner = new MonsterSpawner(random);

            Dungeon dungeon = dungeonGenerator.generate(3);
            monsterSpawner.spawn(dungeon);

            return dungeon;
        }
    }
}