﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Opdracht1
{
    [Serializable]
    class Game
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

            this.startNewGame();
            this.turn();
        }

        public void turn()
        {
            if (this.player.hitPoints < 0)
            {
                this.endOfGame();
            }

            if (this.player.currentNode.packs.Count() > 0)
            {
                this.player.currentNode.doCombat(this.player.currentNode.packs[0], this.player);
                if (this.player.hitPoints < 0)
                {
                    this.endOfGame();
                }
                else this.turn();
            }
            
            else if(this.turnPlayer)
            {
                Node neighbour = this.moveCreatureRandom(this.player.currentNode.getNeighbours());
                this.player.move(neighbour);
                Console.WriteLine("Player moved to: " + neighbour.number);
                if (neighbour == this.dungeon.zones[0].endNode)
                {
                    Console.WriteLine("Player reached the end node of the zone");
                    this.nextDungeon();
                    Console.ReadLine();
                }
                this.turnPlayer = !this.turnPlayer;
                this.turn();
            }
            else
            {
                foreach(Zone zone in this.dungeon.zones)
                {
                    foreach(Node node in zone.nodes)
                    {
                        foreach(Pack pack in node.packs)
                        {
                            List<Node> nodes = pack.getNode().getNeighbours();
                            Node neighbour = this.moveCreatureRandom(nodes);
                            pack.move(neighbour);
                            Console.WriteLine("Pack moved to: " + neighbour.number);
                        }
                    }
                }
                this.turnPlayer = !this.turnPlayer;
                this.turn();
            }
        }

        public Node moveCreatureRandom(List<Node> nodes)
        {
            Random random = new Random(this.t);
            this.t += 24536;
            return nodes[random.Next(nodes.Count)];
        }

        public void endOfGame()
        {
            Console.WriteLine("Player died");
            Console.ReadLine();

            this.startNewGame();
        }

        private void startNewGame()
        {
            this.player = new Player();
            this.nextDungeon();
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
