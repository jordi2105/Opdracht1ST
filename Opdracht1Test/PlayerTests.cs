using Microsoft.VisualStudio.TestTools.UnitTesting;
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
            player.move(dungeon.zones[0].startNode);
            List<Node> allNeigbours = new List<Node>();
            foreach (Node neigbours in player.currentNode.neighbours)
            {
                allNeigbours.Add(neigbours);
            }
            player.getCommand("move", allNeigbours[0], null, false);
            Node current = player.currentNode;
            Assert.AreEqual(allNeigbours[0], current);
        }
    }
}