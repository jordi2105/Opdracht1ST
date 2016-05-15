using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Opdracht1;

namespace Opdracht1Test
{
    [TestClass()]
    public class DungeonGeneratorTests
    {
        [TestMethod()]
        public void dungeon_has_correct_number_of_zones()
        {
            DungeonGenerator dungeonGenerator = this.createDungeonGenerator();

            for (int level = 1; level <= 5; level++) {
                Dungeon dungeon = dungeonGenerator.generate(level);
                Assert.AreEqual(dungeon.zones.Count, level + 1);
            }
        }

        [TestMethod()]
        public void start_node_is_previous_zones_end_node()
        {
            DungeonGenerator dungeonGenerator = this.createDungeonGenerator();
            Dungeon dungeon = dungeonGenerator.generate(1);

            Assert.AreSame(dungeon.zones[0].endNode, dungeon.zones[1].startNode);

        }


        [TestMethod]
        public void average_connectivity_smaller_or_equal_to_three()
        {
            DungeonGenerator dungeonGenerator = this.createDungeonGenerator();
            Dungeon dungeon = dungeonGenerator.generate(3);

            int totalConnections = 0;
            int numberOfNodes = 0;
            foreach (Zone zone in dungeon.zones) {
                foreach (Node node in zone.nodes) {
                    totalConnections += node.neighbourCount();
                    numberOfNodes++;
                }
            }

            Assert.IsTrue((double) totalConnections / numberOfNodes <= 3.0);
        }

        [TestMethod]
        public void is_fully_connected()
        {
            DungeonGenerator dungeonGenerator = this.createDungeonGenerator();
            Dungeon dungeon = dungeonGenerator.generate(3);

            Node startNode = dungeon.zones[0].startNode;
            foreach (Zone zone in dungeon.zones) {
                foreach (Node node in zone.nodes) {
                    Assert.IsTrue(this.connectedToNode(node, startNode, new List<Node>()));
                }
            }
        }

        private bool connectedToNode(Node node, Node connectedTo, List<Node> visited)
        {
            if (visited.Contains(node)) {
                return false;
            }

            visited.Add(node);

            if (node.hasNeighbour(connectedTo)) {
                return true;
            }

            bool connected = false;
            foreach (Node neighbour in connectedTo.getNeighbours()) {
                connected = this.connectedToNode(neighbour, connectedTo, visited) || connected;
            }

            return connected;
        }

        private DungeonGenerator createDungeonGenerator()
        {
            DungeonGenerator dungeonGenerator = new DungeonGenerator(new ZoneGenerator(new Random()));
            return dungeonGenerator;
        }
    }
}