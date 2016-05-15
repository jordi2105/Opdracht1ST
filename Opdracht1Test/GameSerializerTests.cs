using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Opdracht1;

namespace Opdracht1Test
{
    [TestClass]
    public class GameSerializerTests
    {
        [TestMethod]
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

        [TestMethod]
        public void game_get_loaded()
        {
            Game game = this.createGame();

            string fileName = Directory.GetCurrentDirectory() + "/testsave.save";
            game.save(fileName);

            bool isAlive = game.isAlive;
            bool turnPlayer = game.turnPlayer;
            int teller = game.teller;
            int t = game.t;

            FileInfo fileInfo = new FileInfo(fileName);
            if (fileInfo.Exists) {
                game.load(fileName);
                Assert.IsNotNull(game.player);
                Assert.IsNotNull(game.dungeon);
                Assert.AreEqual(isAlive, game.isAlive);
                Assert.AreEqual(turnPlayer, game.turnPlayer);
                Assert.AreEqual(teller, game.teller);
                Assert.AreEqual(t, game.t);
                fileInfo.Delete();
            }
        }

        [TestMethod]
        public void wont_load_non_existent_save()
        {
            Game game = this.createGame();
            bool success = game.load("doesntexists.save");

            Assert.IsFalse(success);
        }

//        private void compareGames(Game one, Game two)
//        {
//            this.compareDungeons(one.dungeon, two.dungeon);
//        }
//
//        private void compareDungeons(Dungeon one, Dungeon two)
//        {
//            Assert.AreEqual(one.level, two.level);
//            Assert.AreEqual(one.zones.Count, two.zones.Count);
//
//            int minCount = Math.Min(one.zones.Count, two.zones.Count);
//            for (int i = 0; i < minCount; i++) {
//                this.compareZones(one.zones[i], two.zones[i]);
//            }
//        }
//
//        private void compareZones(Zone one, Zone two)
//        {
//            Assert.AreEqual(one.number, two.number);
//
//            this.compareNodes(one.startNode, one.startNode);
//            this.compareNodes(one.endNode, one.endNode);
//
//            Assert.AreEqual(one.nodes.Count, two.nodes.Count);
//            int minCount = Math.Min(one.nodes.Count, two.nodes.Count);
//            for (int i = 0; i < minCount; i++) {
//                this.compareNodes(one.nodes[i], two.nodes[i]);
//            }
//        }
//
//        private void compareNodes(Node one, Node two, bool numbersOnly = false)
//        {
//            Assert.AreEqual(one.number, two.number);
//
//            if (!numbersOnly) {
//                this.compareNeighbours(one, two);
//
//                Assert.AreEqual(one.items.Count, two.items.Count);
//                
//            }
//        }
//
//        private void compareNeighbours(Node one, Node two)
//        {
//            Assert.AreEqual(one.neighbours.Count, two.neighbours.Count);
//            int minCount = Math.Min(one.neighbours.Count, two.neighbours.Count);
//            for (int i = 0; i < minCount; i++) {
//                this.compareNodes(one.neighbours[i], two.neighbours[i], true);
//            }
//        }
//

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