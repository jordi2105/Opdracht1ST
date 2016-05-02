using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opdracht1
{
    class Pack
    {
        private List<Monster> monsters;
        private Node currentNode;

        void pack(int n)
        {
            monsters = new List<Monster>();
            for(int i = 0; i < n; i++)
            {
                Monster monster = new Monster();
                monsters.Add(monster);
            }
        }

        void move(Node u)
        {
            currentNode = u;
        }

        void attack(Node x)
        {

        }

    }
}
