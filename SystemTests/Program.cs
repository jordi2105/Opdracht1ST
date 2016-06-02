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
             Random random = new Random(0);

            List<ISpecification> specifications = new List<ISpecification>{
                new MaxMonstersInNode(),
                new MonsterDoesntLeaveZone()
            };

            this.createGameStateDirs(specifications);

            GameSerializer gameSerializer = new GameSerializer(new BinaryFormatter());

            foreach (ISpecification specification in specifications) {
                DirectoryInfo directoryInfo = new DirectoryInfo(this.getGameStatesDir(specification));
                FileInfo[] fileInfos = directoryInfo.GetFiles();
                foreach (FileInfo fileInfo in fileInfos) {
                }
            }

//            new TestGame(
//                new DungeonGenerator(random), 
//                new GameSerializer(new BinaryFormatter()), 
//                new MonsterSpawner(random), 
//                new ItemSpawner(random),
//                random,
//                specifications
//            ).play();
//
//            Console.WriteLine(Directory.GetCurrentDirectory());
//            Console.ReadLine();
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
