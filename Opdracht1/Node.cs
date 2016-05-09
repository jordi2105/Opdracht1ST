using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opdracht1
{
    class Node
    {
        private int count;
        private Zone zone;
        private List<Node> neighbours;

        public Node(int count)
        {
            this.count = count;
            this.neighbours = new List<Node>();
        }

        public void setZone(Zone zone)
        {
            this.zone = zone;
        }

        public void addNeighbour(Node neighbour)
        {
            this.neighbours.Add(neighbour);
            neighbour.neighbours.Add(this);
        }

        public int numNeighbours()
        {
            return this.neighbours.Count;
        }

        public bool hasNeighbour(Node neighbour)
        {
            return this.neighbours.Contains(neighbour);
        }
    }
}
