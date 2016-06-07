using System.Diagnostics;
using Opdracht1;
using Rogue;
using Rogue.DomainObjects;

namespace SystemTests.Specifications
{
    class GuaranteedNumberOfCombats : ISpecification
    {
        Game game;
        int dungeonLevel;
        int numberOfCombatsTemp;
        public bool validate(Game game)
        {
            this.game = game;
            
            if (this.dungeonLevel != game.gameState.dungeon.level)
            {
                if(dungeonLevel > 0 && numberOfCombatsTemp < dungeonLevel + 1)
                {
                    return false;
                }
                this.dungeonLevel = game.gameState.dungeon.level;
            }
            numberOfCombatsTemp = game.gameState.player.numberOfCombatsOfDungeon;
            return true;
        }
    }
}
