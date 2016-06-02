using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rogue;
using Rogue.DomainObjects;
using Rogue.Services;

namespace Opdracht1Test
{
    [TestClass()]
    public class GameSerializerTests
    {
        [TestMethod()]
        public void creates_file_in_file_system()
        {
            Game game = this.createGame();

            string fileName = Directory.GetCurrentDirectory() + "/testsave.save";
            game.save(fileName);

            FileInfo fileInfo = new FileInfo(fileName);
            Assert.IsTrue(fileInfo.Exists);

            if (fileInfo.Exists) {
                fileInfo.Delete();
            }
        }

        [TestMethod()]
        public void game_get_loaded()
        {
            Game game = this.createGame();

            string fileName = Directory.GetCurrentDirectory() + "/testsave.save";
            game.save(fileName);

            bool isAlive = game.gameState.isAlive;
            bool turnPlayer = game.gameState.turnPlayer;
            int teller = game.gameState.teller;

            FileInfo fileInfo = new FileInfo(fileName);
            if (fileInfo.Exists) {
                game.load(fileName);
                Assert.IsNotNull(game.gameState.player);
                Assert.IsNotNull(game.gameState.dungeon);
                Assert.AreEqual(isAlive, game.gameState.isAlive);
                Assert.AreEqual(turnPlayer, game.gameState.turnPlayer);
                Assert.AreEqual(teller, game.gameState.teller);
                fileInfo.Delete();
            }
        }

        [TestMethod()]
        public void wont_load_non_existent_save()
        {
            Game game = this.createGame();
            bool success = game.load("doesntexists.save");

            Assert.IsFalse(success);
        }

        private Game createGame()
        {
            Random random = new Random();
            DungeonGenerator dungeonGenerator = new DungeonGenerator(random);
            MonsterSpawner monsterSpawner = new MonsterSpawner(random);
            ItemSpawner itemSpawner = new ItemSpawner(random);
            GameBuilder gameBuilder = new GameBuilder(dungeonGenerator, monsterSpawner, itemSpawner);
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            GameSerializer gameSerializer = new GameSerializer(binaryFormatter);

            return new Game(gameSerializer,gameBuilder,random);
        }
    }
}