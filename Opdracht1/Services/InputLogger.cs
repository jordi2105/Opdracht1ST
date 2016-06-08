
using System;
using System.IO;

namespace Rogue.Services
{
    public class InputLogger
    {
        private StreamWriter file;

        public string startLogging()
        {
            string fileName = this.getFileName();
            this.file = new StreamWriter(fileName);

            return fileName;
        }

        public void log(string text)
        {
            this.file?.WriteLine(text);
        }

        public void stopLogging()
        {
            this.file.Flush();
            this.file.Close();
            this.file = null;
        }

        private string getFileName()
        {
            return Directory.GetCurrentDirectory() + "\\" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".log";
        }
    }
}
