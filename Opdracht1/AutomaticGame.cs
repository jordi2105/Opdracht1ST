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
        public AutomaticGame(
            DungeonGenerator dungeonGenerator, 
            GameSerializer gameSerializer, 
            MonsterSpawner monsterSpawner, 
            ItemSpawner itemSpawner, 
            Random random) : base(
                dungeonGenerator, 
                gameSerializer, 
                monsterSpawner, 
                itemSpawner, 
                random
        ){}

        public override void turn()
        {
            this.teller++;
            if (this.player.hitPoints < 0)
            {
                this.endOfGame();
            }

            if (this.player.currentNode.packs.Any())
            {
                this.useTimeCrystalOrNot();
                
                this.player.currentNode.doCombat(this.player.currentNode.packs[0], this.player);
                if (this.player.hitPoints < 0) {
                    this.endOfGame();
                }
            }
            
            else if(this.turnPlayer)
            {
                

                List<Node> nodes = this.player.currentNode.neighbours;
                Node neighbour = this.moveCreatureRandom(nodes, null, null);
                if(this.teller > 5000)
                {
                    neighbour = this.dungeon.zones[0].endNode;
                    this.teller = 0;
                }

                this.player.move(neighbour);
                Console.WriteLine("Player moved to: " + neighbour.number);
                
                if (neighbour == this.dungeon.zones[0].endNode)
                {
                    if(this.dungeon.zones.Count() == 1)
                    {
                        Console.WriteLine("Player reached exit node of zone:" + this.player.currentNode.zone.number + " (end of dungeon with level: " + this.dungeon.level + ")");
                        this.nextDungeon();
                        this.player.move(this.dungeon.zones[0].startNode);
                        Console.ReadLine();
                    }

                    else
                    {
                        Console.WriteLine("Player reached the end node of the zone with zonenumber:" + this.player.currentNode.zone.number + "in dungeon with dungeon level: " + this.dungeon.level);
                        //this.player.useTimeCrystal(true, null);
                        Console.ReadLine();
                    }
                    
                }
                this.turnPlayer = !this.turnPlayer;
                //this.turn();
            }
            else
            {
                foreach(Zone zone in this.dungeon.zones)
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
                this.turnPlayer = !this.turnPlayer;
                //this.turn();
            }
        }

        public void useTimeCrystalOrNot()
        {
            foreach (Item item in this.player.bag)
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
