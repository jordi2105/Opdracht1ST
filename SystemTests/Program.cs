using System;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using Rogue;
using Rogue.DomainObjects;
using Rogue.Services;

namespace SystemTests
{
    class Program
    {
        static void Main(string[] args)
        {
            Random random = new Random(0);

            new TestGame(
                new DungeonGenerator(random), 
                new GameSerializer(new BinaryFormatter()), 
                new MonsterSpawner(random), 
                new ItemSpawner(random),
                random,
                new List<ISpecification>{
                    new MaxMonstersInNode()
                }
            ).play();
        }
    }
}
