using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opdracht1
{
    class Pack
    {
        List<Monster> monsters;
        Node currentNode;
        
        public Pack(int n, Node node)
        {
            this.currentNode = node;
            monsters = new List<Monster>();
            for(int i = 0; i < n; i++)
            {
                Monster monster = new Monster(this);
                monsters.Add(monster);
            }
        }

        

        public List<Monster> Monsters
        {
            get
            {
                return this.monsters;
            }
            set
            {
                this.monsters = value;
            }
        }

        public void removeMonster(Monster monster)
        {
            this.monsters.IndexOf(monster);
        }

        void move(Node u) 
        {
            currentNode = u;
        }

        void attack(Player player)
        {
            foreach(Monster monster in monsters)
            {
                player.HitPoints -= monster.AttackRating;
            }
        }

    }
}
