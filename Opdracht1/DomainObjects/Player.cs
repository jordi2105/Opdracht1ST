using System;
using System.Collections.Generic;
using System.Linq;
using Opdracht1;
using Rogue.Services;

namespace Rogue.DomainObjects
{
    [Serializable]
    public class Player : Creature
    {
        public int maxHp = 1000;

        public int killPoints;
        public bool timeCrystalActive;
        public List<Item> bag;
        public Node node { get; set; }
        public Dungeon dungeon;
        private List<Node> visitedNodes;
        private bool safe;
        public bool isAlive = true;

        public Random random;

        public int numberOfCombatsOfDungeon;


        public Player(Random random)
        {
            this.random = random;
            this.killPoints = 0;
            this.attackRating = 5;
            this.hitPoints = this.maxHp;
            this.bag = new List<Item>();
            this.timeCrystalActive = false;
        }

        public void move(Node node)
        {
            this.node = node;
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

        public bool tryMove(int number)
        {
            if (number == this.node.number){
                Console.WriteLine("You stayed in the same node");
                return true;
            }

            Node newNode = this.node.neighbours.Find(n => n.number == number);
            if (newNode == null) {
                Console.WriteLine("Node is not a neighbour, try again");
                return false;
            }

            this.move(newNode);
            return true;
        }

        public void useHealingPotion(HealingPotion healingPotion)
        {
            this.hitPoints =Math.Min(this.maxHp, healingPotion.hitPoints + this.hitPoints);
            this.bag.Remove(healingPotion); 
        }

        public List<HealingPotion> getHealingPotions()
        {
            return this.bag.FindAll(i => i.GetType() == typeof(HealingPotion)).Cast<HealingPotion>().ToList();
        }

        public void useTimeCrystal(TimeCrystal timeCrystal, bool inBattle)
        {
            if (inBattle) {
                this.timeCrystalActive = true;
                
            } else {
                Zone zoneToBeDeleted = this.node.zone;
                this.teleportToSaveNeighbour();
                this.dungeon.zones.Remove(zoneToBeDeleted);
                Console.WriteLine("Bridge and zone removed");
            }
            this.bag.Remove(timeCrystal);
        }

        public TimeCrystal getTimeCrystal()
        {
            return (TimeCrystal) this.bag.Find(i => i.GetType() == typeof(TimeCrystal));
        }

        public void teleportToSaveNeighbour()
        {

            List<Node> nodesToEndNode = getNodesWithShortestPath(this.node, dungeon.zones[this.node.zone.number].endNode);
            foreach (Node node in this.node.neighbours)
                node.neighbours.Remove(this.node);
            this.move(nodesToEndNode[1]);
            
            /*
            //Random random = new Random(13423);
            List<Node> nodesList = new List<Node>();
            
            foreach(Node neighbour in this.node.neighbours)
            {
                if (neighbour.zone == this.node.zone || neighbour.zone == null)
                    continue;
                this.safe = false;
                this.visitedNodes = new List<Node>();
                this.saveNode(neighbour);
                if(this.safe)
                {
                    nodesList.Add(neighbour);
                }
            }
            int index = this.random.Next(0, nodesList.Count());
            foreach(Node node in this.node.neighbours)
                node.neighbours.Remove(this.node);
            this.safe = false;

            this.move(nodesList[index]);*/

        }

        public List<Node> getNodesWithShortestPath(Node startNode, Node endNode)
        {
            Queue<List<Node>> queue = new Queue<List<Node>>();
            List<Node> nodeList = new List<Node>();
            nodeList.Add(startNode);
            queue.Enqueue(nodeList);
            while (queue.Count > 0)
            {
                List<Node> current = queue.Dequeue();
                if (current.Last() == endNode)
                {
                    return current;
                }
                List<Node> neighbours = current.Last().neighbours;
                foreach (Node neighbour in neighbours)
                {
                    List<Node> nodes = new List<Node>(current);
                    nodes.Add(neighbour);
                    queue.Enqueue(nodes);
                }
            }
            return null;
        }

        void saveNode(Node node)
        {
            if (!this.visitedNodes.Exists(x => x == node))
            {
                this.visitedNodes.Add(node);
                foreach (Node neighbour in node.neighbours)
                {
                    if (neighbour == this.node.zone.endNode)
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
