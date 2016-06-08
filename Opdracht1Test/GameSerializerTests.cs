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
            game.initialize();

            string fileName = Directory.GetCurrentDirectory() + "\\testsave.save";
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
            game.initialize();

            string fileName = Directory.GetCurrentDirectory() + "/testsave.save";
            game.save(fileName);

            bool isAlive = game.state.isAlive;
            bool turnPlayer = game.state.turnPlayer;
            int teller = game.state.teller;

            FileInfo fileInfo = new FileInfo(fileName);
            if (fileInfo.Exists) {
                game.load(fileName);
                Assert.IsNotNull(game.state.player);
                Assert.IsNotNull(game.state.dungeon);
                Assert.AreEqual(isAlive, game.state.isAlive);
                Assert.AreEqual(turnPlayer, game.state.turnPlayer);
                Assert.AreEqual(teller, game.state.teller);
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
            PlayerInputReader playerInputReader = new PlayerInputReader();
            DungeonGenerator dungeonGenerator = new DungeonGenerator(random, playerInputReader);
            MonsterSpawner monsterSpawner = new MonsterSpawner(random);
            ItemSpawner itemSpawner = new ItemSpawner(random);
            GameBuilder gameBuilder = new GameBuilder(dungeonGenerator, monsterSpawner, itemSpawner, playerInputReader);
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            GameSerializer gameSerializer = new GameSerializer(binaryFormatter);

            InputLogger inputLogger = new InputLogger();
            return new Game(playerInputReader, gameSerializer,gameBuilder,random, inputLogger);
        }
    }
}