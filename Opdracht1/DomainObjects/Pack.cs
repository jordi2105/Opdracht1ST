using System;
using System.Collections.Generic;
using Opdracht1;

namespace Rogue.DomainObjects
{
    [Serializable]
    public class Pack
    {
        public List<Monster> monsters { get; set; }
        public Node node { get; private set; }

        public bool isMoved;
        
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
            this.node.packs.Remove(this);
            this.node = u;
            u.packs.Add(this);
            
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
