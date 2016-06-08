using System;
using System.IO;

namespace Rogue.Services
{
    public class PlayerInputReader : IInputReader
    {
        private readonly InputLogger inputLogger;

        public PlayerInputReader(InputLogger inputLogger)
        {
            this.inputLogger = inputLogger;
        }

        public string readInput()
        {
            string input = Console.ReadLine().Trim().ToLower();

            this.inputLogger?.log(input);

            return input;
        }
    }
}
