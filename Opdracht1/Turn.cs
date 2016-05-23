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
        public Turn(Game game)
        {
            this.player = game.player;
            this.dungeon = game.dungeon;
            this.game = game;
        }

        public void doTurnPlayer()
        {
            Console.WriteLine("The player has HP: " + player.hitPoints, " KP: " + player.killPoints);
            Console.Write("The player has in his bag: ");
            if (player.bag.Count == 0)
                Console.Write("empty");
            foreach(Item item in player.bag)
            {
                Console.Write(item.getItemType() + ", ");

            }
            Console.WriteLine();
           
            List<Node> neighbours = player.currentNode.neighbours;
            Console.Write("To which node should the player go: ");
            bool first = true;
            foreach(Node neighbour in neighbours)
            {
                if (first)
                    Console.Write(neighbour.number);
                else
                    Console.Write(", " + neighbour.number);
                first = false;
            }
            Console.WriteLine("?");
            int number = int.Parse(Console.ReadLine());
            while(!neighbours.Exists(item => item.number == number))
            {
                Console.WriteLine("Node is not a neighbour, try again");
                number = int.Parse(Console.ReadLine());
            }
            Node node = neighbours.Find(item => item.number == number);
            player.move(node);
            
            
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
                        Node neighbour = game.moveCreatureRandom(nodes, zone, pack);
                        pack.move(neighbour);
                    }
                }
            }
        }

        public bool checkNode()
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

            if (player.currentNode == this.dungeon.zones[0].endNode)
            {
                if (this.dungeon.zones.Count() == 1)
                {
                    Console.WriteLine("This is the exit node of zone:" + this.player.currentNode.zone.number + " (end of dungeon with level: " + this.dungeon.level + ")");
                    game.nextDungeon();
                    this.dungeon = game.dungeon;
                    this.player.move(this.dungeon.zones[0].startNode);

                }

                else
                {
                    Console.WriteLine("This is the end node (bridge) of zone: " + this.player.currentNode.zone.number + " in dungeon with level: " + this.dungeon.level);
                    this.player.useTimeCrystal(true, null);

                }
                return false;
            }

            return true;
        }
    }
}
