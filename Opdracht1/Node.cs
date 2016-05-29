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
        private bool stopCombat = false;

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
            while(pack.Monsters.Count() > 0 && player.hitPoints >= 0 && !stopCombat)
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

            if(stopCombat)
            {
                retreatingToNeighbour(player);
            }

            
        }

        public void retreatingToNeighbour(Player player)
        {
            List<Node> neighbours = player.currentNode.neighbours;
            Console.Write("To which node do you want to go: ");
            bool first = true;
            foreach (Node neighbour in neighbours)
            {
                if (first)
                    Console.Write(neighbour.number);
                else
                    Console.Write(", " + neighbour.number);
                first = false;
            }
            Console.WriteLine("?");
            int number = int.Parse(Console.ReadLine());
            while (!neighbours.Exists(item => item.number == number))
            {
                Console.WriteLine("Node is not a neighbour, try again");
                number = int.Parse(Console.ReadLine());
            }
            
            Node node = neighbours.Find(item => item.number == number);
            player.move(node);
            
        }

        public void doCombatRound(Pack p, Player player)
        {
            Console.WriteLine("Your health is: " + player.hitPoints);
            int totalHealth = 0;
            foreach (Monster monster in p.Monsters)
                totalHealth += monster.hitPoints;
            Console.WriteLine("Enemy has " + p.Monsters.Count + " monsters left with a total health of: " + totalHealth);
            Console.WriteLine("retreat or continue the combat?");
            string input = Console.ReadLine();
            while(input != "continue" && input != "retreat")
            {
                Console.WriteLine("This is not an option!");
                input = Console.ReadLine();
            }
            if (input == "continue")
            {
                player.attack(p.Monsters[0]);
                p.attack(player);
            }
            else stopCombat = true;
           
            
        }
    }
}
