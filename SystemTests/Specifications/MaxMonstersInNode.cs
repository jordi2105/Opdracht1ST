using System.Diagnostics;
using Opdracht1;
using Rogue;
using Rogue.DomainObjects;

namespace SystemTests.Specifications
{
    internal class MaxMonstersInNode : ISpecification
    {
        public bool validate(Game game)
        {
            foreach (Zone zone in game.gameState.dungeon.zones) {
                foreach (Node node in zone.nodes) {
                    int monsterCount = 0;
                    foreach (Pack pack in node.packs) {
                        monsterCount += pack.monsters.Count;
                    }
                    if (this.tooManyMonsters(game.gameState.dungeon.level, monsterCount)) {
                        return false;
                    }
                }
            }

            return true;
        }

        private bool tooManyMonsters(int level, int monsterCount)
        {
            return monsterCount > MonsterSpawner.M*(level + 1);
        }
    }
}