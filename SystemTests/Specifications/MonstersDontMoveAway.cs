using System.Collections.Generic;
using System.Linq;
using Rogue;
using Rogue.DomainObjects;

namespace SystemTests.Specifications
{
    class MonstersDontMoveAway : ISpecification
    {
        private Dictionary<Pack, int> packs;

        public MonstersDontMoveAway()
        {
            this.packs = new Dictionary<Pack, int>();
        }

        public bool validate(Game game)
        {
            if (!this.packs.Any()) {
                this.initialize(game.gameState.dungeon.zones);
                return true;
            }

            foreach (KeyValuePair<Pack, int> pair in this.packs) {
//                pair
            }

            return true;
        }

        private void initialize(List<Zone> zones)
        {
            foreach (Zone zone in zones) {
                foreach (Node node in zone.nodes) {
                    foreach (Pack pack in node.packs) {
//                        this.packs.Add(pack, pack.node);
                    }
                }
            }
        }
    }
}
