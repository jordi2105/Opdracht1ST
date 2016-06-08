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
        private readonly IInputReader playerInputReader;
        private readonly GameSerializer gameSerializer;
        private readonly GameBuilder gameBuilder;
        private readonly Random random;
        private readonly InputLogger inputLogger;

        public GameState state { get; private set; }

        public Game(IInputReader playerInputReader, GameSerializer gameSerializer, GameBuilder gameBuilder, Random random, InputLogger inputLogger)
        {
            this.playerInputReader = playerInputReader;
            this.gameSerializer = gameSerializer;
            this.gameBuilder = gameBuilder;
            this.random = random;
            this.inputLogger = inputLogger;
        }

        public void play()
        {
            

            while (this.state.player.hitPoints > 0) {
                foreach (Zone zone in this.state.dungeon.zones) {
                    foreach (Node node in zone.nodes) {
                        int numMonsters = 0;
                        foreach (Pack pack in node.packs) {
                            numMonsters += pack.monsters.Count;
                        }
                        Console.WriteLine(node.number + ": " + numMonsters);
                    }
                }

                Console.WriteLine();

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
            new Turn(this, this.playerInputReader, this.inputLogger).exec();
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
            if (this.state == null) {
                this.state = this.gameBuilder.buildNewGameState(this.random);
            }
            else
                this.gameBuilder.generateNewDungeon(this.state);
        }
     
    }
}
