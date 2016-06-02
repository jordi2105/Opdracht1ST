using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rogue.DomainObjects;
using Rogue.Services;

namespace Rogue
{
    public class AutomaticGame : Game
    {
        public AutomaticGame(GameSerializer gameSerializer, GameBuilder gameBuilder,Random random) 
            : base(gameSerializer,gameBuilder,random) { }

        public override void turn()
        {
            this.gameState.teller++;
            if (this.gameState.player.hitPoints < 0)
            {
                this.endOfGame();
            }

            if (this.gameState.player.currentNode.packs.Any())
            {
                this.useTimeCrystalOrNot();
                
                this.gameState.player.currentNode.doCombat(this.gameState.player.currentNode.packs[0], this.gameState.player);
                if (this.gameState.player.hitPoints < 0) {
                    this.endOfGame();
                }
            }
            
            else if(this.gameState.turnPlayer)
            {
                

                List<Node> nodes = this.gameState.player.currentNode.neighbours;
                Node neighbour = this.moveCreatureRandom(nodes, null, null);
                if(this.gameState.teller > 5000)
                {
                    neighbour = this.gameState.dungeon.zones[0].endNode;
                    this.gameState.teller = 0;
                }

                this.gameState.player.move(neighbour);
                Console.WriteLine("Player moved to: " + neighbour.number);
                
                if (neighbour == this.gameState.dungeon.zones[0].endNode)
                {
                    if(this.gameState.dungeon.zones.Count() == 1)
                    {
                        Console.WriteLine("Player reached exit node of zone:" + this.gameState.player.currentNode.zone.number + " (end of dungeon with level: " + this.gameState.dungeon.level + ")");
                        this.nextDungeon();
                        this.gameState.player.move(this.gameState.dungeon.zones[0].startNode);
                        Console.ReadLine();
                    }

                    else
                    {
                        Console.WriteLine("Player reached the end node of the zone with zonenumber:" + this.gameState.player.currentNode.zone.number + "in dungeon with dungeon level: " + this.gameState.dungeon.level);
                        //this.player.useTimeCrystal(true, null);
                        Console.ReadLine();
                    }
                    
                }
                this.gameState.turnPlayer = !this.gameState.turnPlayer;
                //this.turn();
            }
            else
            {
                foreach(Zone zone in this.gameState.dungeon.zones)
                {
                    foreach(Node node in zone.nodes)
                    {
                        foreach(Pack pack in node.packs)
                        {
                            List<Node> nodes = pack.node.neighbours;
                            if (!nodes.Any())
                                continue;
                            Node neighbour = this.moveCreatureRandom(nodes, zone, pack);
                            pack.move(neighbour);
                            //Console.WriteLine("Pack moved to: " + neighbour.number);
                        }
                    }
                }
                this.gameState.turnPlayer = !this.gameState.turnPlayer;
                //this.turn();
            }
        }

        public void useTimeCrystalOrNot()
        {
            foreach (Item item in this.gameState.player.bag)
            {
                if (item.GetType() == typeof(TimeCrystal))
                {
                    int timeCrystal = 1;//this.random.Next(0, 4);
                    if (timeCrystal == 1)
                    {
                        //this.player.useTimeCrystal(false, (TimeCrystal)item);
                        break;
                    }

                }
            }
        }
    }
}
