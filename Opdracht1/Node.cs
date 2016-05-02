using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opdracht1
{
    class Node
    {
        private Zone zone;
        private List<Node> neighbours;

        public Node()
        {
            this.neighbours = new List<Node>();
        }

        public void setZone(Zone zone)
        {
            this.zone = zone;
        }

        public void addNeighbour(Node neighbour)
        {
            this.neighbours.Add(neighbour);
        }

        public int NumNeighbours()
        {
            return this.neighbours.Count;
        }
    }
}
