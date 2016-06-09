using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using SystemTests.Specifications;
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

            Random random = new Random(121135371);

            RecordingLoader recordingLoader = new RecordingLoader(Directory.GetCurrentDirectory() + "\\..\\..\\..\\replays");
            GameSerializer gameSerializer = new GameSerializer(new BinaryFormatter());

            while (recordingLoader.next()) {
                IInputReader playerInputReader = new FileInputReader(recordingLoader);
                IGameProvider gameBuilder = new GameLoader(new GameSerializer(new BinaryFormatter()), recordingLoader);

                TestGame testGame = new TestGame(playerInputReader, gameSerializer, gameBuilder, random, new List<ISpecification> {
                    new MaxMonstersInNode(),
                    new MonsterDoesntLeaveZone(),
                    new MonstersDontMoveAway(),
                    new KPAndNumberOfMonsterConstant(),
                    new GuaranteedNumberOfCombats()
                });


                testGame.initialize();
                testGame.play();    
            }
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
