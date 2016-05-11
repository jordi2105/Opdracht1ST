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
        private int number;
        private Zone zone;
        private List<Node> neighbours;

        public Node(int number)
        {
            this.number = number;
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

        public int getNumber()
        {
            return number;
        }

        public List<Node> getNeighbours()
        {
            return neighbours;
        }

        public void removeNeighbour(Node neighbour)
        {
            neighbours.Remove(neighbour);
        }

        public void setNeightbours(List<Node> neighbours)
        {
            this.neighbours = neighbours;
        }
    }
}
