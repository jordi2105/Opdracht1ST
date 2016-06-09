using System;

namespace Rogue.Services
{
    public interface IGameProvider
    {
        GameState build(Random random);
        void generateNewDungeon(GameState gameState);
    }
}