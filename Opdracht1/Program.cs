using System;
using System.Runtime.Serialization.Formatters.Binary;
using Rogue.DomainObjects;
using Rogue.Services;

namespace Rogue
{
    class Program
    {
        static void Main(string[] args)
        {
            Random random = new Random(500);
            InputLogger inputLogger = new InputLogger();
            IInputReader playerInputReader = new PlayerInputReader(inputLogger);
            DungeonGenerator dungeonGenerator = new DungeonGenerator(random, playerInputReader);
            MonsterSpawner monsterSpawner = new MonsterSpawner(random);
            ItemSpawner itemSpawner = new ItemSpawner(random);
            IGameProvider gameBuilder = new GameBuilder(dungeonGenerator, monsterSpawner, itemSpawner);

            GameSerializer gameSerializer = new GameSerializer(new BinaryFormatter());

            Recorder recorder = new Recorder(gameSerializer, inputLogger);
            Game game = new Game(playerInputReader, gameSerializer, gameBuilder, random, recorder);
//            Game game = new AutomaticGame(gameSerializer, gameBuilder, random);
            game.initialize();
            game.play();
        }
    }
}
