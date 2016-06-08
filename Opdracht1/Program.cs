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
            Random random = new Random();
            IInputReader playerInputReader = new PlayerInputReader();
            DungeonGenerator dungeonGenerator = new DungeonGenerator(random, playerInputReader);
            MonsterSpawner monsterSpawner = new MonsterSpawner(random);
            ItemSpawner itemSpawner = new ItemSpawner(random);
            GameBuilder gameBuilder = new GameBuilder(dungeonGenerator, monsterSpawner, itemSpawner, playerInputReader);

            GameSerializer gameSerializer = new GameSerializer(new BinaryFormatter());

            InputLogger inputLogger = new InputLogger();
            Game game = new Game(playerInputReader, gameSerializer, gameBuilder, random, inputLogger);
//            Game game = new AutomaticGame(gameSerializer, gameBuilder, random);
            game.initialize();
            game.play();
        }
    }
}
