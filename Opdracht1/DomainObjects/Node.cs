using System;
using System.Collections.Generic;
using System.Linq;
using Rogue.DomainObjects;
using Rogue.Services;

namespace Opdracht1
{
    [Serializable]
    public class Node
    {
        [NonSerialized] private readonly IInputReader inputReader;

        public int number;
        public Zone zone;
        public List<Pack> packs;
        public List<Node> neighbours;
        public List<Item> items;
        public bool stopCombat = false;
        public bool packRetreated = false;

        public Node(int number, IInputReader inputReader)
        {
            this.number = number;
            this.inputReader = inputReader;
            this.packs = new List<Pack>();
            this.items = new List<Item>();
            this.neighbours = new List<Node>();
        }

        public bool doCombat(Player player)
        {
            return player.node.packs.Any();
        }

        public bool isEndNode()
        {
            return this.zone != null && this.Equals(this.zone.endNode);
        }

        public bool isExitNode()
        {
            return this.isEndNode() && this.zone.isEndZone();
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
            List<Node> neighbours = player.node.neighbours;
            Console.Write("To which node playerTurn you want to go: ");
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
            string[] temp = this.inputReader.readInput().Split();
            while (temp.Length != 1 || (!int.TryParse(temp[0], out output)) || !neighbours.Exists(item => item.number == int.Parse(temp[0])))
            {
                Console.WriteLine("Action is not valid, try another command");
                temp = this.inputReader.readInput().Split();
            }
            
            Node node = neighbours.Find(item => item.number == int.Parse(temp[0]));
            player.move(node);
            
        }

        public void doCombatRound(Pack p, Player player, bool automatic)
        {
           
        }

//        void packAttack(Pack p, Player player)
//        {
//            int totalHealth = 0;
//            foreach(Monster monster in p.monsters)
//            {
//                totalHealth += monster.hitPoints;
//            }
//            if(totalHealth < playerTurn.hitPoints)
//            {
//                packRetreated = true;
//                Console.WriteLine("Pack retreated");
//            }
//            else p.attack(player);
//        }
    }
}