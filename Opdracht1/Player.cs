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
        int killPoints = 0, hpMax = 100;
        bool timeCrystalActive;
        List<Item> bag;
        Node currentNode;
        public Player()
        {
            AttackRating = 30;
            HitPoints = hpMax;
            bag = new List<Item>();
        }

        public void move(Node node)
        {
            currentNode = node;
        }

        public Node getCurrentNode()
        {
            return currentNode;
        }



        public void attack(Monster monster)
        {
            if (timeCrystalActive)
            {
                foreach (Monster _monster in monster.pack.Monsters)
                {
                    _monster.HitPoints -= this.AttackRating;
                    if (monster.HitPoints < 0)
                    {
                        monster.pack.removeMonster(monster);
                        killPoints++;
                    }
                }
            }
            else
            {
                monster.HitPoints -= this.AttackRating;
                if (monster.HitPoints < 0)
                {
                    monster.pack.removeMonster(monster);
                    killPoints++;
                }
            }
            
        }

        public bool getCommand(string command, Node node, Item item, bool usedOnBridge)
        {
            string[] temp = command.Split();
            switch(temp[0])
            {
                case "move":
                    currentNode = node;
                    break;
                case "use-item":
                    if (item.GetType() == typeof(HealingPotion))
                        useHealingPotion();
                    else
                    {
                        useTimeCrystal(usedOnBridge);
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
            this.HitPoints = hpMax;
        }

        void useTimeCrystal(bool usedOnBridge)
        {
            if(usedOnBridge)
            {
                
            }
            else
            {
                timeCrystalActive = true;
            }
        }


    }
}
