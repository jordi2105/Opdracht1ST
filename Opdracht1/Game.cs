using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opdracht1
{
    [Serializable]
    public class Game
    {
        [NonSerialized] private readonly DungeonGenerator dungeonGenerator;
        [NonSerialized] private readonly GameSerializer gameSerializer;
        [NonSerialized] private readonly MonsterSpawner monsterSpawner;
        [NonSerialized] private readonly ItemSpawner itemSpawner;
        [NonSerialized] private readonly Random random;

        public Player player { get; private set; }
        public Dungeon dungeon { get; private set; }

        public bool isAlive { get; private set; }
        public bool turnPlayer { get; set; }
        public int teller { get; set; }
        public int t { get; private set; }

        public Game(
            DungeonGenerator dungeonGenerator, 
            GameSerializer gameSerializer, 
            MonsterSpawner monsterSpawner,
            ItemSpawner itemSpawner,
            Random random,
            bool automatic
        ){
            this.dungeonGenerator = dungeonGenerator;
            this.gameSerializer = gameSerializer;
            this.monsterSpawner = monsterSpawner;
            this.itemSpawner = itemSpawner;
            this.random = random;
            

            this.startNewGame(!automatic);
        }

        public void turn()
        {
            this.teller++;
            if (this.player.hitPoints < 0)
            {
                this.endOfGame();
            }

            if (this.player.currentNode.packs.Count() > 0)
            {
                this.useTimeCrystalOrNot();
                
                this.player.currentNode.doCombat(this.player.currentNode.packs[0], this.player);
                if (this.player.hitPoints < 0)
                {
                    this.endOfGame();
                }
                //else this.turn();
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
                            List<Node> nodes = pack.getNode().neighbours;
                            if (nodes.Count() == 0)
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

        public Node moveCreatureRandom(List<Node> nodes, Zone zone, Pack pack)
        {
            return nodes[this.random.Next(nodes.Count)];
        }

        public void endOfGame()
        {
            this.isAlive = false;
            Console.WriteLine("Player died");
            Console.ReadLine();

            //this.startNewGame();
        }

        public void startNewGame(bool automatic)
        {
            this.turnPlayer = true;
            this.isAlive = true;
            this.teller = 0;
            this.t = 1342342435;

            this.player = new Player();
            this.nextDungeon();
            this.player.dungeon = this.dungeon;
            this.player.move(this.dungeon.zones[0].startNode);

            if (automatic)
                startNewAutomaticGame();
            else
                startNewNonAutomaticGame();
                
           
            
            
            //this.turn();
        }

        public void startNewAutomaticGame()
        {

        }

        public void startNewNonAutomaticGame()
        {
            Turn turn;
            while (player.hitPoints > 0)
            {
                turn = new Turn(this, false);
                turn.doTurnPlayer();
                turn.checkIfCombat();
                if(turn.checkNode())
                {
                    turn = new Turn(this, false);
                    turn.doTurnPacks();
                    turn.checkIfCombat();
                }

                
            }
            this.endOfGame();
        }

        public void save(string fileName)
        {
            this.gameSerializer.save(this, fileName);
        }

        public bool load(string fileName)
        {
            Game loadedGame = this.gameSerializer.load(fileName);

            if (loadedGame == null) {
                return false;
            }

            this.player = loadedGame.player;
            this.dungeon = loadedGame.dungeon;
            this.turnPlayer = loadedGame.turnPlayer;
            this.isAlive = loadedGame.isAlive;
            this.teller = loadedGame.teller;
            this.t = loadedGame.t;

            return true;
        }

        public void nextDungeon()
        {
            this.dungeon = this.dungeonGenerator.generate(this.nextDungeonLevel());
            this.player.dungeon = this.dungeon;
            this.monsterSpawner.spawn(this.dungeon);
            this.itemSpawner.spawn(this.dungeon.zones, this.player.hitPoints);

            Console.WriteLine();
            Console.WriteLine("New dungeon has been generated");
        }

        private int nextDungeonLevel()
        {
            if (this.dungeon == null) {
                return 1;
            }

            return this.dungeon.level + 1;
        }
    }
}
