using System;
using System.Collections.Generic;
using Opdracht1;

namespace Rogue.DomainObjects
{
    public class ItemSpawner
    {
        private readonly Random random;

        public ItemSpawner(Random random)
        {
            this.random = random;
        }

        public void spawn(List<Zone> zones, int playerHitPoints)
        {
            this.spawnTimeCrystal(zones);
            this.spawnHealingPotions(
                playerHitPoints, 
                this.getMonsterHitpoints(zones), 
                zones
            );
        }

        private void spawnTimeCrystal(List<Zone> zones)
        {
            this.getRandomNode(zones).items.Add(new TimeCrystal());
        }

        private void spawnHealingPotions(int playerHitPoints, int monsterHitPoints, List<Zone> zones)
        {
            int numPotions = this.random.Next(1, 10);
            int potionHitpoints = (monsterHitPoints - playerHitPoints)/numPotions;

            if (potionHitpoints < 0) return;

            for (int i = 0; i < numPotions; i++) {
                Node node = this.getRandomNode(zones);
                node.items.Add(new HealingPotion(potionHitpoints));
            }

        }

        private int getMonsterHitpoints(List<Zone> zones)
        {
            int monsterHitPoints = 0;
            foreach (Zone zone in zones) {
                foreach (Node node in zone.nodes) {
                    foreach (Pack pack in node.packs) {
                        foreach (Monster monster in pack.monsters) {
                            monsterHitPoints += monster.hitPoints;
                        }
                    }
                }
            }
            return monsterHitPoints;
        }

        private Node getRandomNode(List<Zone> zones)
        {
            Zone zone = zones[this.random.Next(zones.Count)];

            return zone.nodes[this.random.Next(zone.nodes.Count)];
        }
    }
}
