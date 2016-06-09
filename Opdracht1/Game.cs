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
        private readonly IInputReader inputReader;
        private readonly IGameProvider gameBuilder;
        private readonly Recorder recorder;
        private readonly GameSerializer gameSerializer;
        private readonly Random random;

        public GameState state { get; private set; }

        public Game(IInputReader inputReader, GameSerializer gameSerializer, IGameProvider gameBuilder, Random random, Recorder recorder)
        {
            this.inputReader = inputReader;
            this.gameSerializer = gameSerializer;
            this.gameBuilder = gameBuilder;
            this.random = random;
            this.recorder = recorder;
        }

        public void play()
        {
            while (this.state.player.hitPoints > 0 && this.turn()) {
            }

            this.endOfGame();
        }

        public virtual void initialize()
        {
            this.nextDungeon();
        }

        public virtual bool turn()
        {
            new Turn(this, this.recorder, this.inputReader).exec();

            return true;
        }

        public void endOfGame()
        {
            this.state.isAlive = false;
            Console.WriteLine("Player died");
            Console.ReadLine();
        }

        public Node moveCreatureRandom(List<Node> nodes, Zone zone, Pack pack)
        {
            return nodes[this.random.Next(nodes.Count)];
        }

        public void save(string fileName)
        {
            this.gameSerializer.save(this.state, fileName);
        }

        public bool load(string fileName)
        {
            GameState state = this.gameSerializer.load(fileName);

            if (state == null) {
                return false;
            }

            this.state = state;

            return true;
        }

        public void nextDungeon()
        {
            Console.WriteLine("New dungeon has been generated");
            if (this.state == null) {
                this.state = this.gameBuilder.build(this.random);
            }
            else
                this.gameBuilder.generateNewDungeon(this.state);
        }
     
    }
}
