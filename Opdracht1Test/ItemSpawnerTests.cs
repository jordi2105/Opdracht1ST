using Microsoft.VisualStudio.TestTools.UnitTesting;
using Opdracht1;
using System;
using System.Collections.Generic;

using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Opdracht1.Tests
{
    [TestClass()]
    public class ItemSpawnerTests
    {
        //        [TestMethod()]
        //        public void item_spawner_spawns_correct_health()
        //        {
        //            Random random = new Random();
        //            DungeonGenerator dungeonGenerator = new DungeonGenerator(random);
        //            MonsterSpawner monsterSpawner = new MonsterSpawner(random);
        //            ItemSpawner itemSpawner = new ItemSpawner(random);
        //
        //            Dungeon dungeon = dungeonGenerator.generate(3);
        //            monsterSpawner.spawn(dungeon);
        //            int playerHitPoints = 55;
        //            itemSpawner.spawn(dungeon.zones, playerHitPoints);
        //
        //            int potionHealingPoints = 0;
        //            int monsterHitPoints = 0;
        //            foreach (Zone zone in dungeon.zones) {
        //                foreach (Node node in zone.nodes) {
        //                    foreach (Item item in node.items) {
        //                        if (item.GetType() == typeof(HealingPotion)) {
        //                            HealingPotion healingPotion = (HealingPotion) item;
        //                            potionHealingPoints += healingPotion.hitPoints;
        //                        }
        //                    }
        //                    foreach (Pack pack in node.packs) {
        //                        foreach (Monster monster in pack.Monsters) {
        //                            monsterHitPoints += monster.hitPoints;
        //                        }
        //                    }
        //                }
        //            }
        //
        //            Assert.IsTrue(playerHitPoints + potionHealingPoints <= monsterHitPoints);
        //        }

        [TestMethod()]
        public void player_picks_up_item_if_there_is_one()
        {
            Random random = new Random();
            DungeonGenerator dungeonGenerator = new DungeonGenerator(random);
            Dungeon dungeon = dungeonGenerator.generate(3);
            Player player = new Player();
            List<Node> nodes = dungeon.zones[1].nodes;
            int index = random.Next(0, nodes.Count);
            Node node = nodes[index];
            //player.move(dungeon.zones[0].startNode);
            node.items.Add(new HealingPotion(40));
            player.move(node);

            Assert.IsTrue(node.items.Count == 0);
            

        }
    }
}