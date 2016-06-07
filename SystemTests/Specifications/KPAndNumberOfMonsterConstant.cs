using System.Diagnostics;
using Opdracht1;
using Rogue;
using Rogue.DomainObjects;

namespace SystemTests.Specifications
{
    class KPAndNumberOfMonsterConstant : ISpecification
    {
        int value;
        Game game;
        int dungeonLevel;
        public bool validate(Game game)
        {
            this.game = game;
            if (value == 0 || this.dungeonLevel != game.gameState.dungeon.level)
            {
                value = calculateValue();
                this.dungeonLevel = game.gameState.dungeon.level;
            }
            else if (value != calculateValue())
                return false;
            return true;
        }

        int calculateValue()
        {
            int count = game.gameState.player.killPoints;
            int temp = 0;
            foreach (Zone zone in game.gameState.dungeon.zones)
            {
                foreach (Node node in zone.nodes)
                {
                    foreach (Pack pack in node.packs)
                    {
                        temp += pack.monsters.Count;
                    }
                }
            }
            return temp + count;
        }
    }
}
