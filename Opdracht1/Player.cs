using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opdracht1
{
    [Serializable]
    class Player : Creature
    {
        private const int MaxHp = 100;

        private int killPoints;
        private bool timeCrystalActive;
        private List<Item> bag;
        public Node currentNode { get; private set; }


        public Player()
        {
            this.killPoints = 0;
            this.AttackRating = 5;
            this.HitPoints = MaxHp;
            this.bag = new List<Item>();
        }

        public void move(Node node)
        {
            this.currentNode = node;
        }


        public void attack(Monster monster)
        {
            if (this.timeCrystalActive)
            {
                foreach (Monster monsters in monster.pack.Monsters)
                {
                    monsters.HitPoints -= this.AttackRating;
                    if (monster.HitPoints < 0)
                    {
                        monster.pack.removeMonster(monster);
                        this.killPoints++;
                    }
                }
            }
            else
            {
                monster.HitPoints -= this.AttackRating;
                if (monster.HitPoints < 0)
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
                case "use-item":
                    if (item.GetType() == typeof(HealingPotion))
                        this.useHealingPotion();
                    else
                    {
                        this.useTimeCrystal(usedOnBridge);
                        if(usedOnBridge) return true;
                    }
                    break;
                case "retreat":
                    break;
                default: break;

            }
            return false;
        }

       

        void useHealingPotion()
        {
            this.HitPoints = MaxHp;
        }

        void useTimeCrystal(bool usedOnBridge)
        {
            if(usedOnBridge)
            {
                
            }
            else
            {
                this.timeCrystalActive = true;
            }
        }


    }
}
