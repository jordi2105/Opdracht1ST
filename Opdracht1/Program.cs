﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace Opdracht1
{
    class Program
    {
        static void Main(string[] args)
        {
            Random random = new Random();
            BinaryFormatter formatter = new BinaryFormatter();
            DungeonGenerator dungeonGenerator = new DungeonGenerator(random);
            
            GameSerializer gameSerializer = new GameSerializer(formatter);

            Game game = new Game(dungeonGenerator, gameSerializer);

            game.nextDungeon();
            string fileName = Directory.GetCurrentDirectory() + "my_first_save.save";

            game.save(fileName);
            game.load(fileName);

        }
    }
}
