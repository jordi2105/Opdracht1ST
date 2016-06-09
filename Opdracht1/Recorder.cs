using System;
using System.IO;
using Rogue.Services;

namespace Rogue
{
    public class Recorder
    {
        private readonly InputLogger inputLogger;
        private readonly GameSerializer gameSerializer;

        public Recorder(GameSerializer gameSerializer, InputLogger inputLogger)
        {
            this.gameSerializer = gameSerializer;
            this.inputLogger = inputLogger;
        }

        public void start(GameState state)
        {
            string filename = this.getFileName();
            this.gameSerializer.save(state, filename + ".save");
            this.inputLogger.start(filename + ".input");

            Console.WriteLine("Started recording!");
        }

        public void stop()
        {
            this.inputLogger.stop();
            Console.WriteLine("Stopped recording!");
        }

        private string getFileName()
        {
            string directory = Directory.GetCurrentDirectory() + "\\..\\..\\..\\replays";
            if (!Directory.Exists(directory)) {
                Directory.CreateDirectory(directory);
            }

            return directory + "\\" + DateTime.Now.ToString("yyyyMMddHHmmssfff");
        }
    }
}
