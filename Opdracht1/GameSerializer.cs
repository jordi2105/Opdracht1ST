using System;
using System.IO;
using System.Runtime.Serialization;

namespace Opdracht1
{
    public class GameSerializer
    {
        private readonly IFormatter formatter;

        public GameSerializer(IFormatter formatter)
        {
            this.formatter = formatter;
        }

        public void save(Game game)
        {
            this.save(game, this.getFileName());
        }

        public void save(Game game, string fileName)
        {
            Stream stream = new FileStream(
                    fileName,
                    FileMode.Create,
                    FileAccess.Write,
                    FileShare.None
                    );

            this.formatter.Serialize(stream, game);
            stream.Close();
        }

        public Game load()
        {
            DirectoryInfo directoryInfo = new DirectoryInfo("saves");
            FileInfo[] filesInfos = directoryInfo.GetFiles("*.save");
            FileInfo fileInfo = filesInfos[filesInfos.Length - 1];

            return this.load(fileInfo.FullName);
        }

        
        public Game load(string fileName)
        {
            FileInfo fileInfo = new FileInfo(fileName);
            Stream stream = fileInfo.OpenRead();
            Game game = (Game) this.formatter.Deserialize(stream);
            stream.Close();

            return game;
        }

        private string getFileName()
        {
            return "saves/" + DateTime.Now.ToString("yyyyMMMMddHHmmss") + ".save";
        }

    }
}
