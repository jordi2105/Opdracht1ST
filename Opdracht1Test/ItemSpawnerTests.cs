﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Opdracht1;
using Rogue.DomainObjects;
using Rogue.Services;

namespace Opdracht1Test
{
    [TestClass()]
    public class ItemSpawnerTests
    {
        
//        [TestMethod()]
//        public void item_spawner_spawns_correct_health()
//        {
//            Random random = new Random();
//            IInputReader playerInputReader = new PlayerInputReader();
//            DungeonGenerator dungeonGenerator = new DungeonGenerator(random, playerInputReader);
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
//            foreach (Zone zone in dungeon.zones)
//            {
//                foreach (Node node in zone.nodes)
//                {
//                    foreach (Item item in node.items)
//                    {
//                        if (item.GetType() == typeof(HealingPotion))
//                        {
//                            HealingPotion healingPotion = (HealingPotion)item;
//                            potionHealingPoints += healingPotion.hitPoints;
//                        }
//                    }
//                    foreach (Pack pack in node.packs)
//                    {
//                        foreach (Monster monster in pack.monsters)
//                        {
//                            monsterHitPoints += monster.hitPoints;
//                        }
//                    }
//                }
//            }
//
//            Assert.IsTrue(playerHitPoints + potionHealingPoints <= monsterHitPoints);
//        }
    }
}