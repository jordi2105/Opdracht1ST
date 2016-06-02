using System;
using System.Collections.Generic;

namespace Rogue.DomainObjects
{
    [Serializable]
    public class Pack
    {
        public List<Monster> monsters { get; set; }
        public Node node { get; private set; }
        
        public Pack(int n, Node node)
        {
            this.node = node;
            this.monsters = new List<Monster>();
            for(int i = 0; i < n; i++)
            {
                Monster monster = new Monster(this);
                this.monsters.Add(monster);
            }
        }


        public void removeMonster(Monster monster)
        {
            this.monsters.Remove(monster);
        }

        public void move(Node u) 
        {
            this.node = u;
        }

        public void attack(Player player)
        {
            foreach(Monster monster in this.monsters)
            {
                player.hitPoints -= monster.attackRating;
                if(player.hitPoints < 0)
                {
                    break;
                }
            }
        }

    }
}
