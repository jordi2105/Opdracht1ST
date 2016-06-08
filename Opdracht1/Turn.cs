using System;
using System.Collections.Generic;
using System.Linq;
using Opdracht1;
using Rogue.DomainObjects;
using Rogue.Services;

namespace Rogue
{
    class Turn
    {
        private readonly Game game;
        private readonly PlayerInputReader playerInputReader;

        public Turn(Game game, PlayerInputReader playerInputReader)
        {
            this.game = game;
            this.playerInputReader = playerInputReader;
        }

        public void exec()
        {
            this.writeStatus();
            this.playerTurn();
            this.checkIfCombat();
            if (this.checkNode()) {
                this.packsTurn();
                this.checkIfCombat();
            }
        }

        public void playerTurn()
        {
            Console.WriteLine("What action do you want to do?");
            Player player = this.getPlayer();
            string command = player.getCommand();

            if (command == "record start") {
                string fileName = this.playerInputReader.startLogging();
                this.game.save(fileName.Split('.')[0] + ".save");
                Console.WriteLine("Started recording!");
                this.playerTurn();
                return;
            } else if (command == "record stop") {
                this.playerInputReader.stopLogging();
                Console.WriteLine("Stopped recording!");
            }

            string[] temp = command.Split();

            int output;
            if(temp.Length < 2 || temp[1] == "" || (temp[0] == "move" && !int.TryParse(temp[1], out output)))
            {
                Console.WriteLine("Action is not valid, try another command");
                player.getCommand();
                return;
            }
           

            switch (temp[0])
            {
                case "move":
                    player.tryMove(int.Parse(temp[1]));
                    break;
                case "use-potion":
                    if (temp[1] == "healingpotion" || temp[1] == "HealingPotion")
                        player.useHealingPotion();
                    else if (temp[1] == "timecrystal" || temp[1] == "TimeCrystal" || temp[1] == "timecrystal1")
                    {
                        if(temp[1] == "timecrystal1")
                        {
                            player.useTimeCrystal(true);
                        }
                        else
                        {
                            player.useTimeCrystal(false);
                        }
                    }
                    else
                    {
                        Console.WriteLine("I can't drink a " + temp[1] + ", try again");
                        player.getCommand();
                    }

                    break;
                default:
                    {
                        Console.WriteLine("Action is not valid, try another command");
                        player.getCommand();
                    }
                    break;
            }
        }

        private Player getPlayer()
        {
            return this.game.gameState.player;
        }

        private void writeStatus()
        {
            Player player = this.getPlayer();

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
            foreach (Item item in player.bag) {
                Console.Write(item.getItemType() + ", ");
            }
            Console.ResetColor();
            Console.WriteLine();

            List<Node> neighbours = player.currentNode.neighbours;
            Console.Write("Your neighbours are: ");
            Console.ForegroundColor = ConsoleColor.DarkRed;
            bool first = true;
            foreach (Node neighbour in neighbours) {
                if (first)
                    Console.Write(neighbour.number);
                else
                    Console.Write(", " + neighbour.number);
                first = false;
            }
            Console.ResetColor();
            Console.WriteLine();

        }

        public void packsTurn()
        {
            Dungeon dungeon = this.getDungeon();
            Player player = this.getPlayer();

            foreach (Zone zone in dungeon.zones)
            {
                foreach (Node node in zone.nodes)
                {
                    foreach (Pack pack in node.packs)
                    {
                        List<Node> nodes = pack.node.neighbours;
                        if (!nodes.Any())
                            continue;
                        else if (zone == dungeon.zones[dungeon.zones.Count - 1] && player.currentNode.zone == dungeon.zones[dungeon.zones.Count - 2])
                        {
                            if(zone == player.currentNode.zone)
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
                            while(neighbour.zone != zone && times < 10)
                            {
                                neighbour = this.game.moveCreatureRandom(nodes, zone, pack);
                                times++;
                            }
                            if(neighbour.zone == zone)
                                pack.move(neighbour);*/
                        }
                    }
                }
            }
        }

        private Dungeon getDungeon()
        {
            return this.game.gameState.dungeon;
        }

        public void chasePlayer(Zone zone, Pack pack)
        {
            List<Node> nodesToPlayer = this.getNodesWithShortestPath(pack.node, this.getPlayer().currentNode);
            if(nodesToPlayer[1].zone == zone)
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
                if(shortest[0].zone == zone)
                    pack.move(shortest[0]);
            }
        }

        public void moveMonster(Zone zone, Pack pack)
        {
            Player player = this.getPlayer();
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
            Player player = this.getPlayer();
            if (player.currentNode.packs.Any())
            {
                player.currentNode.doCombat(player.currentNode.packs[0], player, false);
                if (player.hitPoints < 0)
                {
                    this.game.endOfGame();
                }
            }
        }

        public bool checkNode()
        {
            Player player = this.getPlayer();
            Dungeon dungeon = this.getDungeon();
            if (player.currentNode == player.currentNode.zone.endNode)
            {
                if (player.currentNode.zone == dungeon.zones[dungeon.zones.Count - 1])
                {
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    Console.WriteLine("This is the exit node of zone:" + player.currentNode.zone.number + " (end of dungeon with level: " + dungeon.level + ")");
                    Console.ResetColor();
                    this.game.nextDungeon();
                    dungeon = this.game.gameState.dungeon;
                    player.move(dungeon.zones[0].startNode);
                    return false;

                }

                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine("This is the end node (bridge) of zone: " + player.currentNode.zone.number + " in dungeon with level: " + dungeon.level);
                //player.useTimeCrystal(true, null);
                Console.ResetColor();
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
