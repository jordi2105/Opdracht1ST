﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Rogue.DomainObjects
{
    [Serializable]
    public class Node
    {
        public int number;
        public Zone zone;
        public List<Pack> packs;
        public List<Node> neighbours;
        public List<Item> items;
        private bool stopCombat;

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
            while(pack.monsters.Count() > 0 && player.hitPoints >= 0 && !this.stopCombat)
                this.doCombatRound(pack, player);
            
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

            
        }

        public void retreatingToNeighbour(Player player)
        {
            Console.Write("To which node do you want to go: ");
            bool first = true;
            foreach (Node neighbour in player.currentNode.neighbours)
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
            while (temp.Length != 1 || (!int.TryParse(temp[0], out output)) || !player.currentNode.neighbours.Exists(item => item.number == int.Parse(temp[0])))
            {
                Console.WriteLine("Action is not valid, try another command");
                temp = Console.ReadLine().Split();
            }
            
            Node node = player.currentNode.neighbours.Find(item => item.number == int.Parse(temp[0]));
            player.move(node);
            
        }

        public void doCombatRound(Pack p, Player player)
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
            
            if(player.bag.Exists(item => item.GetType() == typeof(TimeCrystal)))
            {
                Console.WriteLine("retreat or continue the combat? Or use a TimeCrystal?");
            }
            else
            {
                Console.WriteLine("retreat or continue the combat?");
            }
            string input = Console.ReadLine();
            while(input != "continue" && input != "retreat" && input != "timecrystal" && input != "TimeCrystal")
            {
                Console.WriteLine("This is not an option!");
                input = Console.ReadLine();
            }

            if (input == "continue") {
                player.attack(p.monsters[0]);
                p.attack(player);
            } else if (input == "retreat") {
                this.stopCombat = true;
            } else if(input == "timecrystal" || input == "TimeCrystal")
            {
                player.getCommand("use-potion timecrystal1");

            }



        }
    }
}
