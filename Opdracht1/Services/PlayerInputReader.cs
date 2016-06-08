using System;
using System.IO;

namespace Rogue.Services
{
    public class PlayerInputReader
    {
        private StreamWriter file;

        public string readInput()
        {
            string readLine = Console.ReadLine();

          
            this.file?.WriteLine(readLine);

            return readLine;
        }

        public string startLogging()
        {
            string fileName = this.getFileName();
            this.file = new StreamWriter(fileName);

            return fileName;
        }

        private string getFileName()
        {
            return Directory.GetCurrentDirectory() + "\\" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".log";
        }

        public void stopLogging()
        {
            this.file.Flush();
            this.file.Close();
            this.file = null;
        }

    }
}
