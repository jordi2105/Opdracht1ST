using System;
using System.Collections.Generic;
using System.Linq;
using Rogue.DomainObjects;

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
        private bool stopCombat = false, packRetreated = false;

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

        public void doCombat(Pack pack, Player player, bool automatic)
        {
            Console.WriteLine("Combat has begon");
            while(pack.monsters.Count() > 0 && player.hitPoints >= 0 && !stopCombat && !packRetreated)
                doCombatRound(pack, player, automatic);
            
            if(pack.monsters.Count() == 0)
            {
                Console.WriteLine("Pack is dead");
                this.packs.Remove(pack);
            }

            if(player.hitPoints <= 0)
            {
                Console.WriteLine("Player is dead");
                player.isAlive = false;
            }

            if(this.stopCombat)
            {
                this.retreatingToNeighbour(player);
                this.stopCombat = false;
                player.timeCrystalActive = false;
            }
            if (this.packRetreated)
            {
                this.retreatPackToNeighbour(pack);
                this.packRetreated = false;
                player.timeCrystalActive = false;
            }

            
        }
        public void retreatPackToNeighbour(Pack pack)
        {
            List<Node> neighbours = pack.node.neighbours;
            for(int i = 0; i < neighbours.Count();i++)
            {
                if (neighbours[i].zone != pack.node.zone)
                    neighbours.Remove(neighbours[i]);
            }
            Random random = new Random(90);
            int index = random.Next(0, neighbours.Count() - 1);
            pack.move(neighbours[index]);
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
             
            int output;
            string[] temp = Console.ReadLine().Split();
            while (temp.Length != 1 || (!int.TryParse(temp[0], out output)) || !neighbours.Exists(item => item.number == int.Parse(temp[0])))
            {
                Console.WriteLine("Action is not valid, try another command");
                temp = Console.ReadLine().Split();
            }
            
            Node node = neighbours.Find(item => item.number == int.Parse(temp[0]));
            player.move(node);
            
        }

        public void doCombatRound(Pack p, Player player, bool automatic)
        {
            if(!automatic)
            {
                Console.Write("Your health is: ");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(player.hitPoints);
                Console.ResetColor();
                int totalHealth = 0;
                foreach (Monster monster in p.monsters)
                    totalHealth += monster.hitPoints;
                Console.Write("Enemy has ");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write(p.monsters.Count);
                Console.ResetColor();
                Console.Write(" monsters left with a total health of: ");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(totalHealth);
                Console.ResetColor();

                if (player.bag.Exists(item => item.GetType() == typeof(TimeCrystal)))
                {
                    Console.WriteLine("retreat or continue the combat? Or use a TimeCrystal?");
                }
                else
                {
                    Console.WriteLine("retreat or continue the combat?");
                }
                string input = Console.ReadLine();
                while (input != "continue" && input != "retreat" && input != "timecrystal" && input != "TimeCrystal")
                {
                    Console.WriteLine("This is not an option!");
                    input = Console.ReadLine();
                }
                if (input == "continue")
                {
                    player.attack(p.monsters[0]);
                    p.attack(player);
                    //packAttack(p, player);
                }
                else if (input == "retreat")
                    stopCombat = true;
                else if (input == "timecrystal" || input == "TimeCrystal")
                {
                    player.getCommand("use-potion timecrystal1");

                }
            }
            else
            {
                player.attack(p.monsters[0]);
                p.attack(player);
                //packAttack(p, player);
            }



        }

        void packAttack(Pack p, Player player)
        {
            int totalHealth = 0;
            foreach(Monster monster in p.monsters)
            {
                totalHealth += monster.hitPoints;
            }
            if(totalHealth < player.hitPoints)
            {
                packRetreated = true;
                Console.WriteLine("Pack retreated");
            }
            else p.attack(player);
        }
    }
}