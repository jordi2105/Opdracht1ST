using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opdracht1
{
    class Player : Creature
    {
        int killPoints = 0, hpMax = 100;
        bool timeCrystalActive;
        List<Item> bag;
        public Player ()
        {
            AttackRating = 5;
            HitPoints = hpMax;
            bag = new List<Item>(); 
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
                    }
                }
            }
            else
            {
                monster.HitPoints -= this.AttackRating;
                if (monster.HitPoints < 0)
                {
                    monster.pack.removeMonster(monster);
                }
            }
            
        }

        public void getCommand(string command)
        {
            string[] temp = command.Split();
            switch(temp[0])
            {
                case "move":
                    break;
                case "use-item":
                    break;
                case "retreat":
                    break;
                default: break;

            }    
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
