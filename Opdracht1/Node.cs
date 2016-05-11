using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opdracht1
{
    [Serializable]
    class Node
    {
        public int number { get; }
        public Zone zone { get; set; }
        public List<Pack> packs { get; }
        public List<Node> neighbours { get; set; }
        public List<Item> items { get; set; }

        public Node(int number)
        {
            this.number = number;
            this.packs = new List<Pack>();
            this.items = new List<Item>();
            this.neighbours = new List<Node>();
        }
    }
}
