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
            for(int i = 100; i < 100; i++)
            {
                DungeonGenerator dungeonGenerator = this.createDungeonGenerator();
                Dungeon dungeon = dungeonGenerator.generate(5);

                int totalConnections = 0;
                int numberOfNodes = 0;
                foreach (Zone zone in dungeon.zones)
                {
                    foreach (Node node in zone.nodes)
                    {
                        totalConnections += node.neighbours.Count;
                        numberOfNodes++;
                    }
                }

                Assert.IsTrue((double)totalConnections / numberOfNodes <= 3.0);
            }
            
        }
        [TestMethod]
        public void number_of_nodes_divided_by_O_is_less_than_maximum_monsters_in_node()
        {
            DungeonGenerator dungeonGenerator = this.createDungeonGenerator();
            Dungeon dungeon = dungeonGenerator.generate(1);
            Random random = new Random();
            MonsterSpawner monsterspawner = new MonsterSpawner(random);
            monsterspawner.spawn(dungeon);
            int nrOfNodes = 0;
            foreach(Zone zone in dungeon.zones)
            {
                nrOfNodes += zone.nodes.Count;
            }
            Assert.IsTrue(nrOfNodes / MonsterSpawner.O <= MonsterSpawner.M * (1 + 1));

        }

        [TestMethod]
//        public void is_fully_connected()
//        {
//            DungeonGenerator dungeonGenerator = this.createDungeonGenerator();
//            Dungeon dungeon = dungeonGenerator.generate(3);
//
//            Node startNode = dungeon.zones[0].startNode;
//            foreach (Zone zone in dungeon.zones) {
//                foreach (Node node in zone.nodes) {
//                    Assert.IsTrue(this.connectedToNode(node, startNode));
//                }
//            }
//        }

        private bool connectedToNode(Node one, Node two)
        {
            if (one.neighbours.Contains(two)) {
                return true;
            }

            bool connected = false;
            foreach (Node neighbour in two.neighbours) {
                connected = this.connectedToNode(neighbour, two) || connected;
            }

            return connected;
        }

      
        private DungeonGenerator createDungeonGenerator()
        {
            DungeonGenerator dungeonGenerator = new DungeonGenerator(new Random());
            return dungeonGenerator;
        }
    }
}