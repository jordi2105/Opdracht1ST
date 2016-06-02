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
        private readonly GameSerializer gameSerializer;
        private readonly GameBuilder gameBuilder;
        private readonly Random random;

        public GameState gameState { get; private set; }

        public Game(GameSerializer gameSerializer, GameBuilder gameBuilder, Random random)
        {
            this.gameSerializer = gameSerializer;
            this.gameBuilder = gameBuilder;
            this.random = random;
        }

        public void play()
        {
            this.initialize();

            while (this.gameState.player.hitPoints > 0){
                this.turn();
            }

            this.endOfGame();
        }

        protected virtual void initialize()
        {
            this.nextDungeon();
        }

        public virtual void turn()
        {
            Turn turn = new Turn(this, false);
            turn.playerTurn();
            turn.checkIfCombat();

            if (turn.checkNode()) {
                turn = new Turn(this, false);
                turn.packsTurn();
                turn.checkIfCombat();
            }
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
                this.gameState = this.gameBuilder.buildNewGameState();
            }

            this.gameBuilder.generateNewDungeon(this.gameState);
        }
     
    }
}
