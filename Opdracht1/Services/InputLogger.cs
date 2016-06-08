
using System;
using System.IO;

namespace Rogue.Services
{
    public class InputLogger
    {
        private StreamWriter file;

        public string start(string fileName)
        {
            this.file = new StreamWriter(fileName);

            return fileName;
        }

        public void log(string text)
        {
            this.file?.WriteLine(text);
        }

        public void stop()
        {
            this.file.Flush();
            this.file.Close();
            this.file = null;
        }

       
    }
}
