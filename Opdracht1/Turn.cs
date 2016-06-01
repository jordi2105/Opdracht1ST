using System;
using System.Collections.Generic;
using System.Linq;
using Rogue.DomainObjects;

namespace Rogue
{
    class Turn
    {
        private readonly Player player;
        private Dungeon dungeon;
        private readonly Game game;

        public Turn(Game game, bool automatic)
        {
            this.player = game.player;
            this.dungeon = game.dungeon;
            this.game = game;
        }

        public void playerTurn()
        {
            Console.Write("Your HP: ");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write(this.player.hitPoints);
            Console.ResetColor();
            Console.Write(" KP: ");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(this.player.killPoints);
            Console.ResetColor();
            Console.Write("You've got in your bag: ");
            Console.ForegroundColor = ConsoleColor.Cyan;
            if (this.player.bag.Count == 0)
                Console.Write("empty");
            foreach(Item item in this.player.bag)
            {
                Console.Write(item.getItemType() + ", ");

            }
            Console.ResetColor();
            Console.WriteLine();
            
            List<Node> neighbours = this.player.currentNode.neighbours;
            Console.Write("Your neighbours are: ");
            Console.ForegroundColor = ConsoleColor.DarkRed;
            bool first = true;
            foreach(Node neighbour in neighbours)
            {
                if (first)
                    Console.Write(neighbour.number);
                else
                    Console.Write(", " + neighbour.number);
                first = false;
            }
            Console.ResetColor();
            Console.WriteLine();
            Console.WriteLine("What action do you want to do?");
            this.player.getCommand(Console.ReadLine());
        }

        public void packsTurn()
        {
            foreach (Zone zone in this.dungeon.zones)
            {
                foreach (Node node in zone.nodes)
                {
                    foreach (Pack pack in node.packs)
                    {
                        List<Node> nodes = pack.getNode().neighbours;
                        if (nodes.Count() == 0)
                            continue;
                        else if (zone == this.dungeon.zones[this.dungeon.zones.Count - 1] && this.player.currentNode.zone == this.dungeon.zones[this.dungeon.zones.Count - 2])
                        {
                            if(zone == this.player.currentNode.zone)
                            {
                                this.chasePlayer(zone, pack);
                            }
                            else
                            {
                                this.moveTowardsShortestPath(zone, pack);
                            }
                        }
                        else if (zone == this.player.currentNode.zone)
                        {
                            this.moveMonster(zone, pack);
                        }
                        else
                        {

                            Node neighbour = this.game.moveCreatureRandom(nodes, zone, pack);
                            int times = 0;
                            while(!(neighbour.zone == zone) && times < 10)
                            {
                                neighbour = this.game.moveCreatureRandom(nodes, zone, pack);
                                times++;
                            }
                            pack.move(neighbour);
                        }
                    }
                }
            }
        }

        public void chasePlayer(Zone zone, Pack pack)
        {
            List<Node> nodesToPlayer = this.getNodesWithShortestPath(pack.getNode(), this.player.currentNode);
            if(nodesToPlayer[1].zone == zone)
                pack.move(nodesToPlayer[1]);
        }

        public void moveTowardsShortestPath(Zone zone, Pack pack)
        {
            List<Node> nodesInPath = this.getNodesWithShortestPath(zone.startNode, zone.endNode);

            if (!nodesInPath.Contains(pack.getNode()))
            {
                List<Node> shortest = null;
                foreach (Node node in nodesInPath)
                {
                    List<Node> nodes = this.getNodesWithShortestPath(pack.getNode(), node);
                    if (shortest == null || nodes.Count < shortest.Count)
                    {
                        shortest = nodes;
                    }
                }
                if(shortest[0].zone == zone)
                    pack.move(shortest[0]);
            }
        }

        public void moveMonster(Zone zone, Pack pack)
        {
            List<Node> nodesToPlayer = this.getNodesWithShortestPath(pack.getNode(), this.player.currentNode);
            List<Node> nodesToEndNode = this.getNodesWithShortestPath(pack.getNode(), zone.endNode);
            if (nodesToEndNode.Count > nodesToPlayer.Count && pack.getNode() != this.player.currentNode)
            {
                pack.move(nodesToPlayer[1]);
            }
            else if (pack.getNode() != zone.endNode)
                pack.move(nodesToEndNode[1]);
        }
        public void checkIfCombat()
        {
            if (this.player.currentNode.packs.Count() > 0)
            {
                //game.useTimeCrystalOrNot();
                this.player.currentNode.doCombat(this.player.currentNode.packs[0], this.player);
                if (this.player.hitPoints < 0)
                {
                    this.game.endOfGame();
                }
            }
        }

        public bool checkNode()
        {
            if (this.player.currentNode == this.player.currentNode.zone.endNode)
            {
                if (this.player.currentNode.zone == this.dungeon.zones[this.dungeon.zones.Count - 1])
                {
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    Console.WriteLine("This is the exit node of zone:" + this.player.currentNode.zone.number + " (end of dungeon with level: " + this.dungeon.level + ")");
                    Console.ResetColor();
                    this.game.nextDungeon();
                    this.dungeon = this.game.dungeon;
                    this.player.move(this.dungeon.zones[0].startNode);
                    return false;

                }

                else
                {
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    Console.WriteLine("This is the end node (bridge) of zone: " + this.player.currentNode.zone.number + " in dungeon with level: " + this.dungeon.level);
                    //this.player.useTimeCrystal(true, null);
                    Console.ResetColor();

                }
            }
            return true;
        }

        public List<Node> getNodesWithShortestPath(Node startNode, Node endNode)
        {
            Queue<List<Node>> queue = new Queue<List<Node>>();
            List<Node> nodeList = new List<Node>();
            nodeList.Add(startNode);
            queue.Enqueue(nodeList);
            while(queue.Count > 0)
            {
                List<Node> current = queue.Dequeue();
                if(current.Last() == endNode)
                {
                    return current;
                }
                List<Node> neighbours = current.Last().neighbours;
                foreach(Node neighbour in neighbours)
                {
                    List<Node> nodes = new List<Node>(current);
                    nodes.Add(neighbour);
                    queue.Enqueue(nodes);
                }
            }
            return null;
        }
    }
}
