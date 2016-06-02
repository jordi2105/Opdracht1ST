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

            DungeonGenerator dungeonGenerator = new DungeonGenerator(random);
            MonsterSpawner monsterSpawner = new MonsterSpawner(random);
            ItemSpawner itemSpawner = new ItemSpawner(random);
            GameBuilder gameBuilder = new GameBuilder(dungeonGenerator, monsterSpawner, itemSpawner);

            new Game(new GameSerializer(new BinaryFormatter()), gameBuilder,random).play();
        }
    }
}
