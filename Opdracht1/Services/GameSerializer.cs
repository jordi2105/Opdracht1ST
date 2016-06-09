using System.IO;
using System.Runtime.Serialization;
using Rogue.DomainObjects;
using System.Xml;
using System.Xml.Serialization;

namespace Rogue.Services
{
    public class GameSerializer
    {
        private readonly IFormatter formatter;

        public GameSerializer(IFormatter formatter)
        {
            this.formatter = formatter;
        }

        public void save(GameState state, string fileName)
        {
            Stream stream = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.None);

            this.formatter.Serialize(stream, state);
            stream.Close();
        }

      

        public GameState load(string fileName)
        {
            FileInfo fileInfo = new FileInfo(fileName);

            if (!fileInfo.Exists) {
                return null;
            }

            Stream stream = fileInfo.OpenRead();
            GameState state = (GameState) this.formatter.Deserialize(stream);
            stream.Close();

            return state;
        }

    }
}
