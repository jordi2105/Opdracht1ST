﻿using System;
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

            new Game(
                new DungeonGenerator(random), 
                new GameSerializer(new BinaryFormatter()), 
                new MonsterSpawner(random), 
                new ItemSpawner(random),
                random
            ).play();
        }
    }
}
