using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opdracht1
{
    [Serializable]
    class Node
    {
        private int number;
        private Zone zone;
        private List<Node> neighbours;
        private List<Pack> packs;

        public Node(int number)
        {
            packs = new List<Pack>();
            this.number = number;
            this.neighbours = new List<Node>();
        }
        
        public List<Pack> getPacks()
        {
            return packs;
        }

        public void addPack(Pack pack)
        {
            packs.Add(pack);
        }

        public void setZone(Zone zone)
        {
            this.zone = zone;
        }

        public void addNeighbour(Node neighbour)
        {
            this.neighbours.Add(neighbour);
            neighbour.neighbours.Add(this);
        }

        public int numNeighbours()
        {
            return this.neighbours.Count;
        }

        public bool hasNeighbour(Node neighbour)
        {
            return this.neighbours.Contains(neighbour);
        }

        public int getNumber()
        {
            return number;
        }

        public List<Node> getNeighbours()
        {
            return neighbours;
        }

        public void removeNeighbour(Node neighbour)
        {
            neighbours.Remove(neighbour);
        }

        public void setNeightbours(List<Node> neighbours)
        {
            this.neighbours = neighbours;
        }

        public void use(Item item)
        {

        }

        public void doCombat(Pack p, Player player)
        {
            Console.WriteLine("Combat has begon");
            while(p.Monsters.Count() > 0 && player.HitPoints >= 0)
                doCombatRound(p, player);
            
            if(p.Monsters.Count() == 0)
            {
                Console.WriteLine("Pack is dead");
                packs.Remove(p);
            }

            
        }

        public void doCombatRound(Pack p, Player player)
        {
            player.attack(p.Monsters[0]);
            p.attack(player);
        }
    }
}
