﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Runtime.Serialization.Formatters.Binary;
using Opdracht1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opdracht1.Tests
{
    [TestClass()]
    public class PlayerTests
    {
        [TestMethod]
        public void healing_potion_heals_player()
        {
            Player player = new Player();
            player.hitPoints = 10;
            player.getCommand("use-potion", null, new HealingPotion(45), false);
            Assert.AreEqual(55, player.hitPoints); 
        }

        [TestMethod]
        public void healing_potion_does_make_player_hp_exceed_max()
        {
            Player player = new Player();
            player.hitPoints = 990;
            player.getCommand("use-potion", null, new HealingPotion(45), false);
            Assert.AreEqual(player.MaxHp, player.hitPoints); 
        }

        [TestMethod]
        public void player_moves_to_right_node()
        {
            Random random = new Random();
            DungeonGenerator dungeonGenerator = new DungeonGenerator(random);
            Dungeon dungeon = dungeonGenerator.generate(3);
            Player player = new Player();
            player.currentNode = dungeon.zones[0].startNode;
            List<Node> allNeigbours = new List<Node>();
            foreach (Node neigbours in player.currentNode.neighbours)
            {
                allNeigbours.Add(neigbours);
            }
            player.getCommand("move", allNeigbours[0], null, false);
            Node current = player.currentNode;
            Assert.AreEqual(allNeigbours[0], current);
        }
        [TestMethod]
        public void time_crystal_used_on_bridge()
        {
            Random random = new Random();
            DungeonGenerator dungeonGenerator = new DungeonGenerator(random);
            Dungeon dungeon = dungeonGenerator.generate(3);
            Player player = new Player();
            player.dungeon = dungeon;
            player.currentNode = dungeon.zones[0].endNode;
            player.getCommand("use-potion", null, new TimeCrystal(), true);
            Assert.IsTrue(player.currentNode != dungeon.zones[0].endNode);
        }
        [TestMethod]
        public void attack_a_pack_player_wins()
        {
            Random random = new Random();
            DungeonGenerator dungeonGenerator = new DungeonGenerator(random);
            Dungeon dungeon = dungeonGenerator.generate(3);
            Player player = new Player();
            player.hitPoints = player.MaxHp;
            player.dungeon = dungeon;
            player.move(dungeon.zones[0].nodes[1]);
            Pack pack = new Pack(3, player.currentNode);
            player.currentNode.doCombat(pack, player);
            Assert.IsTrue(player.currentNode.packs.Count == 0);
        }
        [TestMethod]
        public void attack_a_pack_pack_wins()
        {
            Random random2 = new Random();
            BinaryFormatter formatter = new BinaryFormatter();
            Game game = new Game(
                new DungeonGenerator(random2),
                new GameSerializer(formatter),
                new MonsterSpawner(random2),
                new ItemSpawner(random2)
            );
            Random random = new Random();
            DungeonGenerator dungeonGenerator = new DungeonGenerator(random);
            Dungeon dungeon = dungeonGenerator.generate(3);
            Player player = new Player();
            player.hitPoints = 10;
            player.dungeon = dungeon;
            player.move(dungeon.zones[0].nodes[1]);
            Pack pack = new Pack(3, player.currentNode);
            player.currentNode.doCombat(pack, player);
            Assert.IsTrue(!game.isAlive);
        }
    }
}