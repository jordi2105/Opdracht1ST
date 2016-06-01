using System;
using System.Collections.Generic;
using System.Linq;
using Rogue.DomainObjects;
using Rogue.Services;

namespace Rogue
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

        public Game(
            DungeonGenerator dungeonGenerator, 
            GameSerializer gameSerializer, 
            MonsterSpawner monsterSpawner,
            ItemSpawner itemSpawner,
            Random random
        ){
            this.dungeonGenerator = dungeonGenerator;
            this.gameSerializer = gameSerializer;
            this.monsterSpawner = monsterSpawner;
            this.itemSpawner = itemSpawner;
            this.random = random;
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
        }

        public void play()
        {
            this.initialize();

            while (this.player.hitPoints > 0){
                this.turn();
            }

            this.endOfGame();
        }

        protected virtual void initialize()
        {
            this.turnPlayer = true;
            this.isAlive = true;
            this.teller = 0;

            this.player = new Player();
            this.nextDungeon();
            this.player.dungeon = this.dungeon;
            this.player.move(this.dungeon.zones[0].startNode);
        }

        public virtual void turn()
        {
            Turn turn = new Turn(this, false);
            turn.playerTurn();
            turn.checkIfCombat();
            if(turn.checkNode())
            {
                turn = new Turn(this, false);
                turn.packsTurn();
                turn.checkIfCombat();
            }

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
