using System;
using System.IO;
using Rogue.Services;

namespace SystemTests
{
    class FileInputReader : IInputReader
    {
        private readonly RecordingLoader recordingLoader;
        private readonly StreamReader file;

        public FileInputReader(RecordingLoader recordingLoader)
        {
            this.recordingLoader = recordingLoader;
            this.file = new StreamReader(recordingLoader.getCurrrentInputFile());
        }

        public string readInput()
        {
            string readLine = this.file.ReadLine();

            if (readLine == null) {
                throw new Exception();
            }

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(readLine);
            Console.ResetColor();

            return readLine;
        }
    }
}
