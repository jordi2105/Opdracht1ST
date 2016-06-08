using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemTests
{
    public class RecordingLoader
    {
        private readonly string directory;
        private string[] saveFileNames;
        private string[] inputFileNames;
        private int currentIndex = -1;

        public RecordingLoader(string directory)
        {
            this.directory = directory;
            this.scanForSaves();
        }

        public string getCurrentSaveFile()
        {
            return this.saveFileNames[this.currentIndex];
        }

        public string getCurrrentInputFile()
        {
            return this.inputFileNames[this.currentIndex];
        }

        public bool next()
        {
            return ++this.currentIndex < Math.Min(this.saveFileNames.Length, this.inputFileNames.Length);
        } 

        private void scanForSaves()
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(this.directory);
            this.saveFileNames = directoryInfo.GetFiles("*.save").Select(file => file.FullName).ToArray();
            this.inputFileNames = directoryInfo.GetFiles("*.input").Select(file => file.FullName).ToArray();
        }
    }
}
