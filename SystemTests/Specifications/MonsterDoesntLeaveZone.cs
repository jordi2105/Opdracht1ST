using System;
using System.Collections.Generic;
using System.Linq;
using Opdracht1;
using Rogue;
using Rogue.DomainObjects;

namespace SystemTests.Specifications
{
    class MonsterDoesntLeaveZone : ISpecification
    {
        private Dictionary<Pack, Zone> packs;

        public MonsterDoesntLeaveZone()
        {
            this.packs = new Dictionary<Pack, Zone>();
        }

        public bool validate(Game game)
        {
            if (!this.packs.Any()) {
                this.initialize(game.state.dungeon.zones);
                return true;
            }

            foreach (KeyValuePair<Pack, Zone> pair in this.packs) {
                if (pair.Key.node.zone != null && (!pair.Key.node.zone.Equals(pair.Value))) {
                    return false;
                }
            }

            return true;
        }

        private void initialize(List<Zone> zones)
        {
            foreach (Zone zone in zones) {
                foreach (Node node in zone.nodes) {
                    foreach (Pack pack in node.packs) {
                        this.packs.Add(pack, pack.node.zone);
                    }
                }
            }
        }
    }
}
