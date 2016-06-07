using System;
using System.Collections.Generic;
using System.Linq;
using Opdracht1;

namespace Rogue.DomainObjects
{
    [Serializable]
    public class Player : Creature
    {
        public int maxHp = 10000;

        public int killPoints;
        public bool timeCrystalActive;
        public List<Item> bag;
        public Node currentNode { get; set; }
        public Dungeon dungeon;
        private List<Node> visitedNodes;
        private bool safe = false;
        public bool isAlive = true;

        public Random random;

        public int numberOfCombatsOfDungeon;


        public Player(Random random)
        {
            this.killPoints = 0;
            this.attackRating = 5;
            this.hitPoints = this.maxHp;
            this.bag = new List<Item>();
            this.timeCrystalActive = false;
            this.random = random;
        }

        public void move(Node node)
        {
            this.currentNode = node;
            for (int i = 0; i < node.items.Count; i++)
            {
                this.bag.Add(node.items[i]);
                node.items.Remove(node.items[i]);
            }
            Console.WriteLine("You moved to: " + node.number);

        }

        public void attack(Monster monster)
        {
            if (this.timeCrystalActive)
            {
                for (int index = 0; index < monster.pack.monsters.Count; index++) {
                    Monster curMonster = monster.pack.monsters[index];
                    curMonster.hitPoints -= this.attackRating;
                    if (curMonster.hitPoints < 0)
                    {
                        monster.pack.removeMonster(curMonster);
                        this.killPoints++;
                    }
                }
            }
            else
            {
                monster.hitPoints -= this.attackRating;
                if (monster.hitPoints < 0)
                {
                    monster.pack.removeMonster(monster);
                    this.killPoints++;
                }
            }
            
        }

        public void getCommand(string command/*string command, Node node, Item item, bool usedOnBridge*/)
        {
            int output;
            string[] temp = command.Split();
            if(temp.Length != 2 || temp[1] == "" || (temp[0] == "move" && !int.TryParse(temp[1], out output)))
            {
                Console.WriteLine("Action is not valid, try another command");
                this.getCommand(Console.ReadLine());
            }
            else
            {
                
                switch (temp[0])
                {
                    case "move":
                        this.tryMove(int.Parse(temp[1]));
                        break;
                    case "use-potion":
                        if (temp[1] == "healingpotion" || temp[1] == "HealingPotion")
                            this.useHealingPotion();
                        else if (temp[1] == "timecrystal" || temp[1] == "TimeCrystal" || temp[1] == "timecrystal1")
                        {
                            if(temp[1] == "timecrystal1")
                            {
                                this.useTimeCrystal(true);
                            }
                            else
                            {
                                this.useTimeCrystal(false);
                            }
                        }
                        else
                        {
                            Console.WriteLine("I can't drink a " + temp[1] + ", try again");
                            this.getCommand(Console.ReadLine());
                        }

                        break;
                    default:
                        {
                            Console.WriteLine("Action is not valid, try another command");
                            this.getCommand(Console.ReadLine());
                        }
                        break;
                }
            }
        }

        public void tryMove(int number)
        {
            int oldNumber = this.currentNode.number;
            if (number != this.currentNode.number)
            {
                if (!this.currentNode.neighbours.Exists(item => item.number == number) && number != this.currentNode.number)
                {
                    Console.WriteLine("Node is not a neighbour, try again");
                    this.getCommand(Console.ReadLine());
                }
                else if (number != this.currentNode.number)
                {
                    oldNumber = this.currentNode.number;
                    Node node = this.currentNode.neighbours.Find(item => item.number == number);
                    this.move(node);
                }
            }

            if (number == oldNumber)
            {
                Console.WriteLine("You stayed in the same node");
            }
        }

       

        void useHealingPotion()
        {
            if (!this.bag.Exists(item => item.GetType() == typeof(HealingPotion)))
            {
                Console.WriteLine("You have no healingpotions, try another command");
                this.getCommand(Console.ReadLine());
            }
            else
            {
                Console.WriteLine("Which healingpotion do you want?");
                int i = 0;
                foreach(HealingPotion hp in this.bag)
                {
                    Console.WriteLine(i + ". " + "hp: " + hp.hitPoints);
                    i++;
                }
                int number = int.Parse(Console.ReadLine());
                while(number < 0 || number >= i)
                {
                    number = int.Parse(Console.ReadLine());
                }

                int j = 0;
                foreach (HealingPotion hp in this.bag)
                {
                    if (j == number)
                    {
                        this.hitPoints = Math.Min(this.maxHp, hp.hitPoints + this.hitPoints);
                        this.bag.Remove(hp);
                        break;
                    }
                    j++;
                }
            } 
        }

        public void useTimeCrystal(bool inBattle)
        {
            if(!this.bag.Exists(item => item.GetType() == typeof(TimeCrystal)))
            {
                Console.WriteLine("You have no timecrystal, try another command");
                this.getCommand(Console.ReadLine());
            }

            else if(this.currentNode.zone != null && this.currentNode == this.currentNode.zone.endNode)
            {
                if(!inBattle)
                {
                    Console.WriteLine("Do you want to use this timecrystal on this bridge? (yes/no)");
                    string input = Console.ReadLine();
                    while (input != "yes" && input != "no")
                    {
                        Console.WriteLine("This is not an option, try again (yes/no)");
                        input = Console.ReadLine();
                    }

                    if (input == "yes")
                    {
                        Zone zoneToBeDeleted = this.currentNode.zone;
                        this.teleportToSaveNeighbour();
                        this.dungeon.zones.Remove(zoneToBeDeleted);
                        Console.WriteLine("Bridge and zone removed");
                    }
                    else
                    {
                        this.timeCrystalActive = true;
                        this.bag.RemoveAt(this.bag.IndexOf(this.bag.Find(item => item.GetType() == typeof(TimeCrystal))));
                    }
                }
                else
                {
                    this.timeCrystalActive = true;
                    this.bag.RemoveAt(this.bag.IndexOf(this.bag.Find(item => item.GetType() == typeof(TimeCrystal))));
                }
            }
            
            else
            {
                this.timeCrystalActive = true;
                this.bag.RemoveAt(this.bag.IndexOf(this.bag.Find(item => item.GetType() == typeof(TimeCrystal))));
            }
        }

        void teleportToSaveNeighbour()
        {
            //Random random = new Random(13423);
            List<Node> nodesList = new List<Node>();
            
            foreach(Node neighbour in this.currentNode.neighbours)
            {
                if (neighbour.zone == this.currentNode.zone || neighbour.zone == null)
                    continue;
                this.safe = false;
                this.visitedNodes = new List<Node>();
                this.saveNode(neighbour);
                if(this.safe)
                {
                    nodesList.Add(neighbour);
                }
            }
            int index = random.Next(0, nodesList.Count());
            foreach(Node node in this.currentNode.neighbours)
                node.neighbours.Remove(this.currentNode);
            this.safe = false;

            this.move(nodesList[index]);
        }

        void saveNode(Node node)
        {
            if (!this.visitedNodes.Exists(x => x == node))
            {
                this.visitedNodes.Add(node);
                foreach (Node neighbour in node.neighbours)
                {
                    if (neighbour == this.currentNode.zone.endNode)
                    {
                        this.safe = true; 
                        break;
                    }
                    else this.saveNode(neighbour);
                }
            }
            
        }
    }
}
