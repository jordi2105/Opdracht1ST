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

        private int killPoints;
        public bool timeCrystalActive;
        public List<Item> bag;
        public Node currentNode { get; set; }
        public Dungeon dungeon;
        private List<Node> visitedNodes;
        private bool safe = false;


        public Player()
        {
            this.killPoints = 0;
            this.AttackRating = 5;
            this.hitPoints = MaxHp;
            this.bag = new List<Item>();
        }

        public void move(Node node)
        {
            this.currentNode = node;
            foreach (Item item in node.items)
            {
                //bag.Add(item);
                //node.items.Remove(item);
                Console.WriteLine(item);
            }
           
            for (int i = 0; i < node.items.Count; i++)
            {
                bag.Add(node.items[i]);
                node.items.Remove(node.items[i]);
                Console.WriteLine("delete item");
            }
            Console.WriteLine(node.items.Count);
                
        }

        public void attack(Monster monster)
        {
            if (this.timeCrystalActive)
            {
                foreach (Monster monsters in monster.pack.Monsters)
                {
                    monsters.hitPoints -= this.AttackRating;
                    if (monster.hitPoints < 0)
                    {
                        monster.pack.removeMonster(monster);
                        this.killPoints++;
                    }
                }
                timeCrystalActive = false;
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

        public bool getCommand(string command, Node node, Item item, bool usedOnBridge)
        {
            string[] temp = command.Split();
            switch(temp[0])
            {
                case "move":
                    this.currentNode = node;
                    break;
                case "use-potion":
                    if (item.GetType() == typeof(HealingPotion))
                        this.useHealingPotion((HealingPotion) item);
                    else
                    {
                        this.useTimeCrystal(usedOnBridge, null);
                        if(usedOnBridge) return true;
                    }
                    break;
                case "retreat":
                    break;

            }
            return false;
        }

       

        void useHealingPotion(HealingPotion potion)
        {
            this.hitPoints = Math.Min(MaxHp, potion.hitPoints + this.hitPoints);
            
        }

        public void useTimeCrystal(bool usedOnBridge, TimeCrystal timeCrystal)
        {
            if(usedOnBridge)
            {
                Zone zoneToBeDeleted = currentNode.zone;
                teleportToSaveNeighbour();
                dungeon.zones.Remove(zoneToBeDeleted);
                Console.WriteLine("Bridge and zone removed");
            }
            else
            {
                this.timeCrystalActive = true;
                bag.Remove(timeCrystal);
            }
        }

        void teleportToSaveNeighbour()
        {
            Random random = new Random(13423);
            List<Node> nodesList = new List<Node>();
            
            foreach(Node neighbour in currentNode.neighbours)
            {
                if (neighbour.zone == currentNode.zone)
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
           
            currentNode = nodesList[index];
            
            Console.WriteLine("Player teleported to node: " + currentNode.number);
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
