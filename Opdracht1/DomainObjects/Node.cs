using System;
using System.Collections.Generic;
using System.Linq;
using Rogue.DomainObjects;
using Rogue.Services;

namespace Opdracht1
{
    [Serializable]
    public class Node
    {
        public int number;
        public Zone zone;
        public List<Pack> packs;
        public List<Node> neighbours;
        public List<Item> items;
        public bool stopCombat = false;
        public bool packRetreated = false;

        public Node(int number)
        {
            this.number = number;
            this.packs = new List<Pack>();
            this.items = new List<Item>();
            this.neighbours = new List<Node>();
        }

        public bool doCombat(Player player)
        {
            return player.node.packs.Any();
        }

        public bool isEndNode()
        {
            return this.zone != null && this.Equals(this.zone.endNode);
        }

        public bool isExitNode()
        {
            return this.isEndNode() && this.zone.isEndZone();
        }
    }
}