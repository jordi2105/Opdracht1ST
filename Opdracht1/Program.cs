﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opdracht1
{
    class Program
    {
        static void Main(string[] args)
        {
            Random random = new Random();
            DungeonGenerator dungeonGenerator = new DungeonGenerator(random);
            Dungeon dungeon = dungeonGenerator.generate(1);

            int i = 0;
        }
    }
}
