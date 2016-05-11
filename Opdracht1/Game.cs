using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opdracht1
{
    [Serializable]
    class Game
    {
        [NonSerialized] private DungeonGenerator dungeonGenerator;
        [NonSerialized] private GameSerializer gameSerializer;

        private Player player;
        private Dungeon dungeon;
        private List<Pack> packs;
        private List<Item> items;
        private bool turnPlayer = true;
        private int t = 1342342435;
        

        public Game(DungeonGenerator dungeonGenerator, GameSerializer gameSerializer)
        {

            player = new Player();
            this.dungeonGenerator = dungeonGenerator;
            nextDungeon();
            player.move(dungeon.getZones()[0].getStartNode());
            //ItemSpawner itemSpawner
            this.gameSerializer = gameSerializer;
            turn();
        }

        public void turn()
        {
            if (player.HitPoints < 0)
            {
                endOfGame();
            }

            if (player.getCurrentNode().getPacks().Count() > 0)
            {
                player.getCurrentNode().doCombat(player.getCurrentNode().getPacks()[0], player);
                if (player.HitPoints < 0)
                {
                    endOfGame();
                }
                else turn();
            }
            
            else if(turnPlayer)
            {
                List<Node> nodes = player.getCurrentNode().getNeighbours();
                Node neighbour = moveCreatureRandom(nodes);
                player.move(neighbour);
                Console.WriteLine("Player moved to: " + neighbour.getNumber());
                if (neighbour == dungeon.getZones()[0].getEndNode())
                {
                    Console.WriteLine("Player reached the end node of the zone");
                    nextDungeon();
                    Console.ReadLine();
                }
                turnPlayer = !turnPlayer;
                turn();
            }
            else
            {
                foreach(Zone zone in dungeon.getZones())
                {
                    foreach(Node node in zone.getNodes())
                    {
                        foreach(Pack pack in node.getPacks())
                        {
                            List<Node> nodes = pack.getNode().getNeighbours();
                            Node neighbour = moveCreatureRandom(nodes);
                            pack.move(neighbour);
                            Console.WriteLine("Pack moved to: " + neighbour.getNumber());
                        }
                    }
                }
                turnPlayer = !turnPlayer;
                turn();
            }
           
            
        }

        

        public Node moveCreatureRandom(List<Node> nodes)
        {
            Random random = new Random(t);
            t += 24536;
            int index = random.Next(0, nodes.Count());
            Node neighbour = nodes[index];
            return neighbour;
        }

        public void endOfGame()
        {
            Console.WriteLine("Player died");
            Console.ReadLine();
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
            this.packs = loadedGame.packs;
            this.items = loadedGame.items;

            return true;
        }

        

        public void nextDungeon()
        {
            this.dungeon = this.dungeonGenerator.generate(this.nextDungeonLevel());
        }

        private int nextDungeonLevel()
        {
            if (this.dungeon == null) {
                return 1;
            }

            return this.dungeon.getLevel() + 1;
        }
    }
}
