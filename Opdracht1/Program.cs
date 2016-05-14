using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Opdracht1
{
    class Program
    {
        static void Main(string[] args)
        {
            Random random = new Random();
            BinaryFormatter formatter = new BinaryFormatter();

            Game game = new Game(
                new DungeonGenerator(new ZoneGenerator(random)), 
                new GameSerializer(formatter), 
                new MonsterSpawner(random), 
                new ItemSpawner(random)
            );

            game.turn();
            string fileName = Directory.GetCurrentDirectory() + "my_first_save.save";

            game.save(fileName);
            game.load(fileName);

        }
    }
}
