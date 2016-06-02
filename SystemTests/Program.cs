using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using SystemTests.Specifications;
using Rogue;
using Rogue.DomainObjects;
using Rogue.Services;

namespace SystemTests
{
    class Program
    {
        static void Main(string[] args)
        {
            new Program().run();  
        }

        public void run()
        {

            List<ISpecification> specifications = new List<ISpecification>{
                new MaxMonstersInNode(),
                new MonsterDoesntLeaveZone()
            };

            Random random = new Random(0);

            DungeonGenerator dungeonGenerator = new DungeonGenerator(random);
            MonsterSpawner monsterSpawner = new MonsterSpawner(random);
            ItemSpawner itemSpawner = new ItemSpawner(random);
            GameBuilder gameBuilder = new GameBuilder(dungeonGenerator, monsterSpawner, itemSpawner);

            new TestGame(new GameSerializer(new BinaryFormatter()), gameBuilder, random, specifications).play();

        }

        private void createGameStateDirs(List<ISpecification> specifications)
        {
            foreach (ISpecification specification in specifications) {
                string gameStatesDir = this.getGameStatesDir(specification);
                if (!Directory.Exists(gameStatesDir)) Directory.CreateDirectory(gameStatesDir);
            }
        }

        private string getGameStatesDir(ISpecification specification)
        {
            return Directory.GetCurrentDirectory() + "\\..\\..\\GameStates\\" + specification.GetType().Name;
        }
    }
}
