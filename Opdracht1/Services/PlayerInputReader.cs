using System;
using System.IO;

namespace Rogue.Services
{
    public class PlayerInputReader : IInputReader
    {

        public string readInput()
        {
            return Console.ReadLine().Trim().ToLower();
        }
    }
}
