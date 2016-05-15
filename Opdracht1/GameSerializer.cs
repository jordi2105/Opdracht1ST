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
        
        public Game load(string fileName)
        {
            FileInfo fileInfo = new FileInfo(fileName);

            if (!fileInfo.Exists) {
                return null;
            }

            Stream stream = fileInfo.OpenRead();
            Game game = (Game) this.formatter.Deserialize(stream);
            stream.Close();

            return game;
        }
    }
}
