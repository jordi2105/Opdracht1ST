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
        public int number { get; }
        public Zone zone { get; set; }
        public List<Pack> packs { get; }

        private List<Node> neighbours;
        public List<Item> items { get; set; }

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
            while (pack.Monsters.Count() > 0 && player.hitPoints >= 0)
                doCombatRound(pack, player);

            if (pack.Monsters.Count() == 0) {
                Console.WriteLine("Pack is dead");
                packs.Remove(pack);
            }


        }

        public void doCombatRound(Pack p, Player player)
        {
            player.attack(p.Monsters[0]);
            p.attack(player);
        }

        public void addNeighbour(Node neighbour)
        {
            this.neighbours.Add(neighbour);
            neighbour.neighbours.Add(this);
        }

        public List<Node> getNeighbours()
        {
            return this.neighbours;
        }

        public void setNeighbours(List<Node> neighbours)
        {
            this.neighbours = neighbours;
        }

        public int neighbourCount()
        {
            return this.neighbours.Count;
        }

        public bool hasNeighbour(Node neighbour)
        {
            return this.neighbours.Contains(neighbour);
        }
    }
}
