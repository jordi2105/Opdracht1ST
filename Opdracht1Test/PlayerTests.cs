using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Opdracht1;
using Rogue.DomainObjects;
using Rogue.Services;

namespace Opdracht1Test
{
    [TestClass()]
    public class PlayerTests
    {
        [TestMethod]
        public void healing_potion_heals_player()
        {
            Player player = new Player();
            player.hitPoints = 10;
//            player.getCommand("use-potion", null, new HealingPotion(45), false);
            Assert.AreEqual(55, player.hitPoints); 
        }

        [TestMethod]
        public void healing_potion_does_make_player_hp_exceed_max()
        {
            Player player = new Player();
            player.hitPoints = 990;
//            player.getCommand("use-potion", null, new HealingPotion(45), false);
            Assert.AreEqual(player.maxHp, player.hitPoints); 
        }

        [TestMethod]
        public void player_moves_to_right_node()
        {
            Random random = new Random();
            DungeonGenerator dungeonGenerator = new DungeonGenerator(random);
            Dungeon dungeon = dungeonGenerator.generate(3);
            Player player = new Player();
            player.move(dungeon.zones[0].startNode);
            List<Node> allNeigbours = new List<Node>();
            foreach (Node neigbours in player.currentNode.neighbours)
            {
                allNeigbours.Add(neigbours);
            }
//            player.getCommand("move", allNeigbours[0], null, false);
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
            Node node = dungeon.zones[0].endNode;
            player.currentNode = dungeon.zones[0].endNode;
//            player.getCommand("use-potion", null, new TimeCrystal(), true);
            Assert.IsTrue(player.currentNode != node);
        }
        [TestMethod]
        public void time_crystal_not_used_on_bridge()
        {
            Random random = new Random();
            DungeonGenerator dungeonGenerator = new DungeonGenerator(random);
            Dungeon dungeon = dungeonGenerator.generate(3);
            Player player = new Player();
            player.dungeon = dungeon;
            TimeCrystal timecrystal = new TimeCrystal();
//            player.getCommand("use-potion", null, timecrystal, false);
            Assert.IsTrue(player.timeCrystalActive && !player.bag.Contains(timecrystal));
        }
        [TestMethod]
        public void attack_a_pack_player_wins()
        {
            Random random = new Random();
            DungeonGenerator dungeonGenerator = new DungeonGenerator(random);
            Dungeon dungeon = dungeonGenerator.generate(3);
            Player player = new Player();
            player.hitPoints = player.maxHp;
            player.dungeon = dungeon;
            player.move(dungeon.zones[0].nodes[1]);
            Pack pack = new Pack(3, player.currentNode);
            player.currentNode.doCombat(pack, player, true);
            Assert.IsTrue(player.currentNode.packs.Count == 0);
        }
        [TestMethod]
        public void attack_a_pack_pack_wins()
        {

            Random random = new Random();
            DungeonGenerator dungeonGenerator = new DungeonGenerator(random);
            Dungeon dungeon = dungeonGenerator.generate(3);
            Player player = new Player();
            player.hitPoints = 1;
            player.dungeon = dungeon;
            player.currentNode = dungeon.zones[0].nodes[1];
            player.timeCrystalActive = false;
            Pack pack = new Pack(15, player.currentNode);
            player.currentNode.doCombat(pack, player, true);
            Assert.IsFalse(player.isAlive);
        }

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