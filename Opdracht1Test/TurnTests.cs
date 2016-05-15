using Microsoft.VisualStudio.TestTools.UnitTesting;
using Opdracht1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization.Formatters.Binary;

namespace Opdracht1.Test
{
    [TestClass]
    public class TurnTests
    {

        [TestMethod()]
        public void turn_test_player_not_in_startnode()
        {
            Game game = this.createGame();

            game.turn();

            Assert.IsTrue(game.player.currentNode != game.dungeon.zones[0].startNode);
        }

        [TestMethod()]
        public void turn_test_pack()
        {
            Game game = this.createGame();
            game.turnPlayer = false;

            game.turn();

            Assert.IsTrue(true);
        }

        [TestMethod()]
        public void pack_and_player_at_same_node_one_will_remain()
        {
            Game game = this.createGame();
            game.player.currentNode = game.dungeon.zones[0].startNode.neighbours[0];
            game.player.currentNode.packs.Add(new Pack(10, game.player.currentNode));
            game.turn();

            Assert.IsTrue(!game.isAlive ^ game.player.currentNode.packs.Count == 0);
        }

        [TestMethod()]
        public void player_at_end_node_uses_timecrystal_on_bridge()
        {
            Game game = this.createGame();
            
            game.player.currentNode = game.dungeon.zones[1].endNode;
            Node node = game.player.currentNode;
            game.turn();

            Assert.IsFalse(game.dungeon.zones[0].nodes.Contains(node));
        }

        [TestMethod()]
        public void use_time_crystal_if_in_bag()
        {
            Game game = this.createGame();
            TimeCrystal timecrystal = new TimeCrystal();
            game.player.bag.Clear();
            game.player.bag.Add(timecrystal);
            game.useTimeCrystalOrNot();

            Assert.IsTrue(!game.player.bag.Contains(timecrystal));
        }

        [TestMethod()]
        public void end_of_game()
        {
            Game game = this.createGame();
            game.endOfGame();

            Assert.IsFalse(game.isAlive);
        }



        private Game createGame()
        {
            Random random = new Random();
            DungeonGenerator dungeonGenerator = new DungeonGenerator(random);
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            GameSerializer gameSerializer = new GameSerializer(binaryFormatter);
            MonsterSpawner monsterSpawner = new MonsterSpawner(random);
            ItemSpawner itemSpawner = new ItemSpawner(random);

            Game game = new Game(
                dungeonGenerator,
                gameSerializer,
                monsterSpawner,
                itemSpawner,
                random);
            return game;
        }
    }
}
