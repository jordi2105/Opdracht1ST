using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opdracht1
{
    [Serializable]
    public class Player : Creature
    {
        public int MaxHp = 1000;

        public int killPoints;
        public bool timeCrystalActive;
        public List<Item> bag;
        public Node currentNode { get; set; }
        public Dungeon dungeon;
        private List<Node> visitedNodes;
        private bool safe = false;
        public bool isAlive = true;


        public Player()
        {
            this.killPoints = 0;
            this.AttackRating = 5;
            this.hitPoints = MaxHp;
            this.bag = new List<Item>();
            timeCrystalActive = false;
        }

        public void move(Node node)
        {
            this.currentNode = node;
            for (int i = 0; i < node.items.Count; i++)
            {
                bag.Add(node.items[i]);
                node.items.Remove(node.items[i]);
            }
            Console.WriteLine("You moved to: " + node.number);

        }

        public void attack(Monster monster)
        {
            if (this.timeCrystalActive)
            {
                for (int index = 0; index < monster.pack.Monsters.Count; index++)
                {
                    Monster curMonster = monster.pack.Monsters[index];
                    curMonster.hitPoints -= this.AttackRating;
                    if (curMonster.hitPoints < 0)
                    {
                        monster.pack.removeMonster(curMonster);
                        this.killPoints++;
                    }
                }
                
            }
            else
            {
                monster.hitPoints -= this.AttackRating;
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
                getCommand(Console.ReadLine());
            }
            else
            {
                
                switch (temp[0])
                {
                    case "move":
                        tryMove(int.Parse(temp[1]));
                        break;
                    case "use-potion":
                        if (temp[1] == "healingpotion" || temp[1] == "HealingPotion")
                            useHealingPotion();
                        else if (temp[1] == "timecrystal" || temp[1] == "TimeCrystal" || temp[1] == "timecrystal1")
                        {
                            if(temp[1] == "timecrystal1")
                            {
                                useTimeCrystal(true);
                            }
                            else
                            {
                                useTimeCrystal(false);
                            }
                            
                        }
                        else
                        {
                            Console.WriteLine("I can't drink a " + temp[1] + ", try again");
                            getCommand(Console.ReadLine());
                        }

                        break;
                    default:
                        {
                            Console.WriteLine("Action is not valid, try another command");
                            getCommand(Console.ReadLine());
                        }
                        break;
                }
            }
        }

        public void tryMove(int number)
        {
            int oldNumber = currentNode.number;
            if (number != currentNode.number)
            {
                if (!currentNode.neighbours.Exists(item => item.number == number) && number != currentNode.number)
                {
                    Console.WriteLine("Node is not a neighbour, try again");
                    getCommand(Console.ReadLine());
                }
                else if (number != currentNode.number)
                {
                    oldNumber = currentNode.number;
                    Node node = currentNode.neighbours.Find(item => item.number == number);
                    move(node);
                }
            }

            if (number == oldNumber)
            {
                Console.WriteLine("You stayed in the same node");
            }
        }

       

        void useHealingPotion()
        {
            if (!bag.Exists(item => item.GetType() == typeof(HealingPotion)))
            {
                Console.WriteLine("You have no healingpotions, try another command");
                getCommand(Console.ReadLine());
            }
            else
            {
                Console.WriteLine("Which healingpotion do you want?");
                int i = 0;
                foreach(HealingPotion hp in bag)
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
                foreach (HealingPotion hp in bag)
                {
                    if (j == number)
                    {
                        this.hitPoints = Math.Min(MaxHp, hp.hitPoints + this.hitPoints);
                        bag.Remove(hp);
                        break;
                    }
                    j++;
                }
            } 
        }

        public void useTimeCrystal(bool inBattle)
        {
            if(!bag.Exists(item => item.GetType() == typeof(TimeCrystal)))
            {
                Console.WriteLine("You have no timecrystal, try another command");
                getCommand(Console.ReadLine());
            }

            else if(currentNode.zone != null && currentNode == currentNode.zone.endNode)
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
                        Zone zoneToBeDeleted = currentNode.zone;
                        teleportToSaveNeighbour();
                        dungeon.zones.Remove(zoneToBeDeleted);
                        Console.WriteLine("Bridge and zone removed");
                    }
                    else
                    {
                        this.timeCrystalActive = true;
                        bag.RemoveAt(bag.IndexOf(bag.Find(item => item.GetType() == typeof(TimeCrystal))));
                    }
                }
                else
                {
                    this.timeCrystalActive = true;
                    bag.RemoveAt(bag.IndexOf(bag.Find(item => item.GetType() == typeof(TimeCrystal))));
                }
            }
            
            else
            {
                this.timeCrystalActive = true;
                bag.RemoveAt(bag.IndexOf(bag.Find(item => item.GetType() == typeof(TimeCrystal))));
            }
        }

        void teleportToSaveNeighbour()
        {
            Random random = new Random(13423);
            List<Node> nodesList = new List<Node>();
            
            foreach(Node neighbour in currentNode.neighbours)
            {
                if (neighbour.zone == currentNode.zone || neighbour.zone == null)
                    continue;
                safe = false;
                visitedNodes = new List<Node>();
                saveNode(neighbour);
                if(safe)
                {
                    nodesList.Add(neighbour);
                }
            }
            int index = random.Next(0, nodesList.Count());
            foreach(Node node in currentNode.neighbours)
                node.neighbours.Remove(currentNode);
            safe = false;
           
            move(nodesList[index]);
        }

        void saveNode(Node node)
        {
            if (!visitedNodes.Exists(x => x == node))
            {
                visitedNodes.Add(node);
                foreach (Node neighbour in node.neighbours)
                {
                    if (neighbour == currentNode.zone.endNode)
                    {
                        safe = true; 
                        break;
                    }
                    else saveNode(neighbour);
                }
            }
            
        }
    }
}
