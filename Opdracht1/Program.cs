﻿using System;
using System.Runtime.Serialization.Formatters.Binary;

namespace Opdracht1
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
                random,
                true
            );
        }
    }
}
