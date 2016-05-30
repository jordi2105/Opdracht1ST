using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opdracht1
{
    class Turn
    {
        Player player;
        Dungeon dungeon;
        Game game;
        public Turn(Game game, bool automatic)
        {
            this.player = game.player;
            this.dungeon = game.dungeon;
            this.game = game;
        }

        public void doTurnPlayer()
        {
            Console.Write("Your HP: ");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write(player.hitPoints);
            Console.ResetColor();
            Console.Write(" KP: ");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(player.killPoints);
            Console.ResetColor();
            Console.Write("You've got in your bag: ");
            Console.ForegroundColor = ConsoleColor.Cyan;
            if (player.bag.Count == 0)
                Console.Write("empty");
            foreach(Item item in player.bag)
            {
                Console.Write(item.getItemType() + ", ");

            }
            Console.ResetColor();
            Console.WriteLine();
            
            List<Node> neighbours = player.currentNode.neighbours;
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
            player.getCommand(Console.ReadLine());

           



        }

        public void doTurnPacks()
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
                        else if (zone == player.currentNode.zone)
                        {
                            List<Node> nodesToPlayer = getNodeWithShortestPath(pack.getNode(), player.currentNode);
                            List<Node> nodesToEndNode = getNodeWithShortestPath(pack.getNode(), zone.endNode);
                            if (nodesToEndNode.Count > nodesToPlayer.Count && pack.getNode() != player.currentNode)
                            {
                                pack.move(nodesToPlayer[1]);
                            }
                            else if(pack.getNode() != zone.endNode)
                                pack.move(nodesToEndNode[1]);
                        }
                        else
                        {
                            Node neighbour = game.moveCreatureRandom(nodes, zone, pack);
                            pack.move(neighbour);
                        }
                    }
                }
            }
        }

        public bool checkNode(bool checkEndNode)
        {
            if (this.player.currentNode.packs.Count() > 0)
            {
                //game.useTimeCrystalOrNot();
                this.player.currentNode.doCombat(this.player.currentNode.packs[0], this.player);
                if (this.player.hitPoints < 0)
                {
                    game.endOfGame();
                }
                return true;
            }

            if (checkEndNode && player.currentNode == this.dungeon.zones[0].endNode)
            {
                if (this.dungeon.zones.Count() == 1)
                {
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    Console.WriteLine("This is the exit node of zone:" + this.player.currentNode.zone.number + " (end of dungeon with level: " + this.dungeon.level + ")");
                    Console.ResetColor();
                    game.nextDungeon();
                    this.dungeon = game.dungeon;
                    this.player.move(this.dungeon.zones[0].startNode);

                }

                else
                {
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    Console.WriteLine("This is the end node (bridge) of zone: " + this.player.currentNode.zone.number + " in dungeon with level: " + this.dungeon.level);
                    //this.player.useTimeCrystal(true, null);
                    Console.ResetColor();

                }
                return false;
            }

            return true;
        }

        public List<Node> getNodeWithShortestPath(Node startNode, Node endNode)
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
