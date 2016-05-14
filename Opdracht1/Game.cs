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

        private Dungeon dungeon;
        private List<Pack> packs;
        private List<Item> items;
        private bool turnPlayer = true;
        private int t = 1342342435;
        private Player player;
        private int teller = 0;
        private Random random;
        private bool isAlive = true;

        public Game(
            DungeonGenerator dungeonGenerator, 
            GameSerializer gameSerializer, 
            MonsterSpawner monsterSpawner,
            ItemSpawner itemSpawner
        ){
            this.dungeonGenerator = dungeonGenerator;
            this.gameSerializer = gameSerializer;
            this.monsterSpawner = monsterSpawner;
            this.itemSpawner = itemSpawner;

            random = new Random();
            this.startNewGame();
            while(isAlive)
            {
                this.turn();
            }
            
        }

        public void turn()
        {
            teller++;
            if (this.player.hitPoints < 0)
            {
                this.endOfGame();
            }

            if (this.player.currentNode.packs.Count() > 0)
            {

                useTimeCrystalOrNot();
                
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
                if(teller > 5000)
                {
                    neighbour = this.dungeon.zones[0].endNode;
                    teller = 0;
                }

                this.player.move(neighbour);
                Console.WriteLine("Player moved to: " + neighbour.number);

                
                
                if (neighbour == this.dungeon.zones[0].endNode)
                {
                    if(dungeon.zones.Count() == 1)
                    {
                        Console.WriteLine("Player reached exit node of zone:" + player.currentNode.zone.number + " (end of dungeon with level: " + dungeon.level + ")");
                        this.nextDungeon();
                        player.move(dungeon.zones[0].startNode);
                        Console.ReadLine();
                    }

                    else
                    {
                        Console.WriteLine("Player reached the end node of the zone with zonenumber:" + player.currentNode.zone.number + "in dungeon with dungeon level: " + dungeon.level);
                        player.useTimeCrystal(true, null);
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
            foreach (Item item in player.bag)
            {
                if (item.GetType() == typeof(TimeCrystal))
                {
                    int timeCrystal = random.Next(0, 4);
                    if (timeCrystal == 1)
                    {
                        player.useTimeCrystal(false, (TimeCrystal)item);
                        break;
                    }

                }
            }
        }

        public Node moveCreatureRandom(List<Node> nodes, Zone zone, Pack pack)
        {
            return nodes[random.Next(nodes.Count)];
        }

        public void endOfGame()
        {
            isAlive = false;
            Console.WriteLine("Player died");
            Console.ReadLine();

            //this.startNewGame();
        }

        public void startNewGame()
        {
            isAlive = true;
            this.player = new Player();
            this.nextDungeon();
            player.dungeon = dungeon;
            this.player.move(this.dungeon.zones[0].startNode);
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

            return true;
        }

        public void nextDungeon()
        {
            this.dungeon = this.dungeonGenerator.generate(this.nextDungeonLevel());
            player.dungeon = dungeon;
            this.monsterSpawner.spawn(this.dungeon);
            this.itemSpawner.spawn(this.dungeon.zones, this.player.hitPoints);
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
