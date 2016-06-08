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

            Random random = new Random(1211);

            Random random = new Random(0);
            PlayerInputReader playerInputReader = new PlayerInputReader();
            DungeonGenerator dungeonGenerator = new DungeonGenerator(random, playerInputReader);
            MonsterSpawner monsterSpawner = new MonsterSpawner(random);
            ItemSpawner itemSpawner = new ItemSpawner(random);
            GameBuilder gameBuilder = new GameBuilder(dungeonGenerator, monsterSpawner, itemSpawner, playerInputReader);

            List<ISpecification> specifications = new List<ISpecification>();
            MaxMonstersInNode spec = new MaxMonstersInNode();
            specifications.Add(spec);
            MonsterDoesntLeaveZone spec2 = new MonsterDoesntLeaveZone();
            specifications.Add(spec2);
            MonstersDontMoveAway spec3 = new MonstersDontMoveAway();
            specifications.Add(spec3);
            KPAndNumberOfMonsterConstant spec4 = new KPAndNumberOfMonsterConstant();
            specifications.Add(spec4);
            GuaranteedNumberOfCombats spec5 = new GuaranteedNumberOfCombats();
            specifications.Add(spec5);
            TestGame testGame = new TestGame(new GameSerializer(new BinaryFormatter()), gameBuilder, random, specifications);
            testGame.initialize();
            testGame.play();

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
