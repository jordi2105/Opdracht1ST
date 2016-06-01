﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opdracht1
{
    class TurnBot
    {
        Player player;
        Dungeon dungeon;
        Game game;
        public TurnBot(Game game)
        {
            this.player = game.player;
            this.dungeon = game.dungeon;
            this.game = game;
        }

        public void doTurnPlayer(int counter)
        {
            /*Console.Write("Your HP: ");
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
            foreach (Item item in player.bag)
            {
                Console.Write(item.getItemType() + ", ");

            }
            Console.ResetColor();
            Console.WriteLine();

            List<Node> neighbours = player.currentNode.neighbours;
            Console.Write("Your neighbours are: ");
            Console.ForegroundColor = ConsoleColor.DarkRed;
            bool first = true;
            foreach (Node neighbour in neighbours)
            {
                if (first)
                    Console.Write(neighbour.number);
                else
                    Console.Write(", " + neighbour.number);
                first = false;
            }
            Console.ResetColor();
            Console.WriteLine();*/
           // Console.WriteLine("What action do you want to do?");

            if(counter > 500 && player.currentNode.zone != null)
            {
                List<Node> nodes;
                if (player.currentNode == player.currentNode.zone.endNode)
                {
                    nodes = getNodesWithShortestPath(player.currentNode, game.dungeon.zones[player.currentNode.zone.number + 1].endNode);
                }
                else nodes = getNodesWithShortestPath(player.currentNode, player.currentNode.zone.endNode);
                player.getCommand("move" + " " + nodes[1].number.ToString());
                Console.WriteLine("player teleported to end node: " + nodes[1].number.ToString());
            }
            else
            {
                List<Node> neighbours = player.currentNode.neighbours;
                player.getCommand("move" + " " + randomNeighbourNumber(neighbours).ToString());
            }
           
        }

        public int randomNeighbourNumber(List<Node> neighbours)
        {
            Node node = game.moveCreatureRandom(neighbours, player.currentNode.zone, null);
            while(node.neighbours.Count < 2 || (player.currentNode.zone != null && player.currentNode.zone.endNode == player.currentNode && node.zone == player.currentNode.zone))
            {
                node = game.moveCreatureRandom(neighbours, player.currentNode.zone, null);
            }
            return node.number;
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
                        else if (zone == dungeon.zones[dungeon.zones.Count - 1] && player.currentNode.zone == dungeon.zones[dungeon.zones.Count - 2])
                        {
                            if (zone == player.currentNode.zone)
                            {
                                chasePlayer(zone, pack);
                            }
                            else
                            {
                                moveTowardsShortestPath(zone, pack);
                            }
                        }
                        else if (zone == player.currentNode.zone)
                        {
                            moveMonster(zone, pack);
                        }
                        else
                        {

                            Node neighbour = game.moveCreatureRandom(nodes, zone, pack);
                            int times = 0;
                            while (!(neighbour.zone == zone) && times < 10)
                            {
                                neighbour = game.moveCreatureRandom(nodes, zone, pack);
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
            List<Node> nodesToPlayer = getNodesWithShortestPath(pack.getNode(), player.currentNode);
            if (nodesToPlayer[1].zone == zone)
                pack.move(nodesToPlayer[1]);
        }

        public void moveTowardsShortestPath(Zone zone, Pack pack)
        {
            List<Node> nodesInPath = getNodesWithShortestPath(zone.startNode, zone.endNode);

            if (!nodesInPath.Contains(pack.getNode()))
            {
                List<Node> shortest = null;
                foreach (Node node in nodesInPath)
                {
                    List<Node> nodes = getNodesWithShortestPath(pack.getNode(), node);
                    if (shortest == null || nodes.Count < shortest.Count)
                    {
                        shortest = nodes;
                    }
                }
                if (shortest[0].zone == zone)
                    pack.move(shortest[0]);
            }
        }

        public void moveMonster(Zone zone, Pack pack)
        {
            List<Node> nodesToPlayer = getNodesWithShortestPath(pack.getNode(), player.currentNode);
            List<Node> nodesToEndNode = getNodesWithShortestPath(pack.getNode(), zone.endNode);
            if (nodesToEndNode.Count > nodesToPlayer.Count && pack.getNode() != player.currentNode)
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
                this.player.currentNode.doCombat(this.player.currentNode.packs[0], this.player, true);
                if (this.player.hitPoints < 0)
                {
                    game.endOfGame();
                }
            }
        }

        public bool checkNode()
        {
            if (player.currentNode.zone != null && player.currentNode == player.currentNode.zone.endNode)
            {
                if (player.currentNode.zone == this.dungeon.zones[dungeon.zones.Count - 1])
                {
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    Console.WriteLine("This is the exit node of zone:" + this.player.currentNode.zone.number + " (end of dungeon with level: " + this.dungeon.level + ")");
                    Console.ResetColor();
                    game.nextDungeon();
                    this.dungeon = game.dungeon;
                    this.player.move(this.dungeon.zones[0].startNode);
                    Console.ReadLine();
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
            while (queue.Count > 0)
            {
                List<Node> current = queue.Dequeue();
                if (current.Last() == endNode)
                {
                    return current;
                }
                List<Node> neighbours = current.Last().neighbours;
                foreach (Node neighbour in neighbours)
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