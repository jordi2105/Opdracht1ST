using System;
using Rogue;
using Rogue.Services;

namespace SystemTests
{
    public class GameLoader : IGameProvider
    {
        private readonly RecordingLoader recordingLoader;
        private readonly GameSerializer gameSerializer;
        
        public GameLoader(GameSerializer gameSerializer, RecordingLoader recordingLoader)
        {
            this.gameSerializer = gameSerializer;
            this.recordingLoader = recordingLoader;
        }

        public GameState build(Random random)
        {
            return this.gameSerializer.load(this.recordingLoader.getCurrentSaveFile());
        }

        public void generateNewDungeon(GameState gameState)
        {
            throw new NotImplementedException();
        }
    }
}
