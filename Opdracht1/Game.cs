using System;
using System.Collections.Generic;
using System.Linq;
using Opdracht1;
using Rogue.DomainObjects;
using Rogue.Services;

namespace Rogue
{
    public class Game
    {
//        private readonly  L
        private readonly PlayerInputReader playerInputReader;
        private readonly GameSerializer gameSerializer;
        private readonly GameBuilder gameBuilder;
        private readonly Random random;

        public GameState gameState { get; private set; }

        public Game(PlayerInputReader playerInputReader, GameSerializer gameSerializer, GameBuilder gameBuilder, Random random)
        {
            this.playerInputReader = playerInputReader;
            this.gameSerializer = gameSerializer;
            this.gameBuilder = gameBuilder;
            this.random = random;
        }

        public void play()
        {
            while (this.gameState.player.hitPoints > 0){
                this.turn();
            }

            this.endOfGame();
        }

        public virtual void initialize()
        {
            this.nextDungeon();
        }

        public virtual void turn()
        {
            new Turn(this, this.playerInputReader).exec();
        }

        public void endOfGame()
        {
            this.gameState.isAlive = false;
            Console.WriteLine("Player died");
            Console.ReadLine();
        }

        public Node moveCreatureRandom(List<Node> nodes, Zone zone, Pack pack)
        {
            return nodes[this.random.Next(nodes.Count)];
        }

        public void save(string fileName)
        {
            this.gameSerializer.save(this.gameState, fileName);
        }

        public bool load(string fileName)
        {
            GameState state = this.gameSerializer.load(fileName);

            if (state == null) {
                return false;
            }

            this.gameState = state;

            return true;
        }

        public void nextDungeon()
        {
            if (this.gameState == null) {
                this.gameState = this.gameBuilder.buildNewGameState(random);
            }
            else
                this.gameBuilder.generateNewDungeon(this.gameState);
        }
     
    }
}
