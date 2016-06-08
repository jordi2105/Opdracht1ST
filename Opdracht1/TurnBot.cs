using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rogue;
using Rogue.DomainObjects;

namespace Opdracht1
{
    class TurnBot
    {
        Player player;
        Dungeon dungeon;
        Game game;

        public TurnBot(Game game)
        {
            player = game.gameState.player;
            this.dungeon = game.gameState.dungeon;
            this.game = game;
        }

        public void doTurnPlayer(int counter)
        {
            if(counter > 500 && player.currentNode.zone != null)
            {
                List<Node> nodes;
                if (player.currentNode == player.currentNode.zone.endNode)
                {
                    nodes = this.getNodesWithShortestPath(
                        player.currentNode, 
                        this.game.gameState.dungeon.zones[player.currentNode.zone.number + 1].endNode);
                }
                else nodes = this.getNodesWithShortestPath(player.currentNode, player.currentNode.zone.endNode);
                player.getCommand();
                Console.WriteLine("playerTurn teleported to end node: " + nodes[1].number.ToString());
            }
            else
            {
                List<Node> neighbours = player.currentNode.neighbours;
                player.getCommand();
            }
           
        }

        public int randomNeighbourNumber(List<Node> neighbours)
        {
            Node node = this.game.moveCreatureRandom(neighbours, this.player.currentNode.zone, null);
            while(node.neighbours.Count < 2 || (this.player.currentNode.zone != null && this.player.currentNode.zone.endNode == this.player.currentNode && (node.zone == this.player.currentNode.zone || node.zone == null)))
            {
                node = this.game.moveCreatureRandom(neighbours, player.currentNode.zone, null);
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
                        List<Node> nodes = pack.node.neighbours;
                        if (nodes.Count() == 0)
                            continue;
                        else if (zone == this.dungeon.zones[this.dungeon.zones.Count - 1] && player.currentNode.zone == this.dungeon.zones[this.dungeon.zones.Count - 2])
                        {
                            if (zone == player.currentNode.zone)
                            {
                                this.chasePlayer(zone, pack);
                            }
                            else
                            {
                                this.moveTowardsShortestPath(zone, pack);
                            }
                        }
                        else if (zone == player.currentNode.zone)
                        {
                            this.moveMonster(zone, pack);
                        }
                        else
                        {
                            /*
                            Node neighbour = this.game.moveCreatureRandom(nodes, zone, pack);
                            int times = 0;
                            while (neighbour.zone != zone && times < 10)
                            {
                                neighbour = this.game.moveCreatureRandom(nodes, zone, pack);
                                times++;
                            }
                            if(neighbour.zone == zone)
                                pack.move(neighbour);
                                */
                        }
                    }
                }
            }
        }

        public void chasePlayer(Zone zone, Pack pack)
        {
            List<Node> nodesToPlayer = this.getNodesWithShortestPath(pack.node, player.currentNode);
            if (nodesToPlayer[1].zone == zone)
                pack.move(nodesToPlayer[1]);
        }

        public void moveTowardsShortestPath(Zone zone, Pack pack)
        {
            List<Node> nodesInPath = this.getNodesWithShortestPath(zone.startNode, zone.endNode);

            if (!nodesInPath.Contains(pack.node))
            {
                List<Node> shortest = null;
                foreach (Node node in nodesInPath)
                {
                    List<Node> nodes = this.getNodesWithShortestPath(pack.node, node);
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
            List<Node> nodesToPlayer = this.getNodesWithShortestPath(pack.node, player.currentNode);
            List<Node> nodesToEndNode = this.getNodesWithShortestPath(pack.node, zone.endNode);
            if (nodesToEndNode.Count > nodesToPlayer.Count && pack.node != this.player.currentNode && nodesToPlayer[1].zone == pack.node.zone)
            {
                pack.move(nodesToPlayer[1]);
            }
            else if (pack.node != zone.endNode && nodesToEndNode[1].zone == pack.node.zone)
                pack.move(nodesToEndNode[1]);
        }
        public void checkIfCombat()
        {
            if (player.currentNode.packs.Count() > 0)
            {
                //game.useTimeCrystalOrNot();
                player.currentNode.doCombat(
                    player.currentNode.packs[0], 
                    player, 
                    true
                );
                
            }
        }

        public bool checkNode()
        {
            if (player.currentNode.zone != null && player.currentNode == player.currentNode.zone.endNode)
            {
                if (player.currentNode.zone == this.dungeon.zones[this.dungeon.zones.Count - 1])
                {
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    Console.WriteLine("This is the exit node of zone:" + player.currentNode.zone.number + " (end of dungeon with level: " + this.dungeon.level + ")");
                    Console.ResetColor();
                    this.game.nextDungeon();
                    this.dungeon = this.game.gameState.dungeon;
                   // this.player.move(this.dungeon.zones[0].startNode);
                    Console.ReadLine();
                    return false;

                }

                else
                {
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    Console.WriteLine("This is the end node (bridge) of zone: " + player.currentNode.zone.number + " in dungeon with level: " + this.dungeon.level);
                    //playerTurn.useTimeCrystal(true, null);
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
