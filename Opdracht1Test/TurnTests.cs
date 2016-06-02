using System;
using System.Runtime.Serialization.Formatters.Binary;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rogue;
using Rogue.DomainObjects;
using Rogue.Services;

namespace Opdracht1Test
{
    [TestClass]
    public class TurnTests
    {

        [TestMethod()]
        public void turn_test_player_not_in_startnode()
        {
            Game game = this.createGame();

            game.turn();

            Assert.IsTrue(game.gameState.player.currentNode != game.gameState.dungeon.zones[0].startNode);
        }

        [TestMethod()]
        public void turn_test_pack()
        {
            Game game = this.createGame();
            game.gameState.turnPlayer = false;

            game.turn();

            Assert.IsTrue(true);
        }

        [TestMethod()]
        public void pack_and_player_at_same_node_one_will_remain()
        {
            Game game = this.createGame();
            game.gameState.player.currentNode = game.gameState.dungeon.zones[0].startNode.neighbours[0];
            game.gameState.player.currentNode.packs.Add(new Pack(10, game.gameState.player.currentNode));
            game.turn();

            Assert.IsTrue(!game.gameState.isAlive ^ game.gameState.player.currentNode.packs.Count == 0);
        }

        [TestMethod()]
        public void player_at_end_node_uses_timecrystal_on_bridge()
        {
            Game game = this.createGame();
            
            game.gameState.player.currentNode = game.gameState.dungeon.zones[1].endNode;
            Node node = game.gameState.player.currentNode;
            game.turn();

            Assert.IsFalse(game.gameState.dungeon.zones[0].nodes.Contains(node));
        }

        [TestMethod()]
        public void use_time_crystal_if_in_bag()
        {
            AutomaticGame game = this.createGame();
            TimeCrystal timecrystal = new TimeCrystal();
            game.gameState.player.bag.Clear();
            game.gameState.player.bag.Add(timecrystal);
            game.useTimeCrystalOrNot();

            Assert.IsTrue(!game.gameState.player.bag.Contains(timecrystal));
        }

        [TestMethod()]
        public void end_of_game()
        {
            Game game = this.createGame();
            game.endOfGame();

            Assert.IsFalse(game.gameState.isAlive);
        }

        private AutomaticGame createGame()
        {
            Random random = new Random();

            DungeonGenerator dungeonGenerator = new DungeonGenerator(random);
            MonsterSpawner monsterSpawner = new MonsterSpawner(random);
            ItemSpawner itemSpawner = new ItemSpawner(random);
            GameBuilder gameBuilder = new GameBuilder(dungeonGenerator, monsterSpawner, itemSpawner);

            BinaryFormatter binaryFormatter = new BinaryFormatter();
            GameSerializer gameSerializer = new GameSerializer(binaryFormatter);

            return new AutomaticGame(gameSerializer,gameBuilder,random);
        }
    }
}
