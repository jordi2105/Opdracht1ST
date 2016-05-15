using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opdracht1
{
    [Serializable]
    public class Node
    {
        public int number;
        public Zone zone;
        public List<Pack> packs;
        public List<Node> neighbours;
        public List<Item> items;

        public Node(int number)
        {
            this.number = number;
            this.packs = new List<Pack>();
            this.items = new List<Item>();
            this.neighbours = new List<Node>();
        }

        public void use(Item item)
        {
            
        }

        public void doCombat(Pack pack, Player player)
        {
            Console.WriteLine("Combat has begon");
            while(pack.Monsters.Count() > 0 && player.hitPoints >= 0)
                doCombatRound(pack, player);
            
            if(pack.Monsters.Count() == 0)
            {
                Console.WriteLine("Pack is dead");
                packs.Remove(pack);
            }

            if(player.hitPoints <= 0)
            {
                Console.WriteLine("Player is dead");
                player.isAlive = false;
            }

            
        }

        public void doCombatRound(Pack p, Player player)
        {
            player.attack(p.Monsters[0]);
            p.attack(player);
        }
    }
}
