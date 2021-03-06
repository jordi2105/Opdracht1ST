﻿using System;
using Rogue.DomainObjects;

namespace Rogue.Services
{
    public class GameBuilder : IGameProvider
    {
        private readonly DungeonGenerator dungeonGenerator;
        private readonly MonsterSpawner monsterSpawner;
        private readonly ItemSpawner itemSpawner;
        private Random random;

        public GameBuilder(DungeonGenerator dungeonGenerator, MonsterSpawner monsterSpawner, ItemSpawner itemSpawner)
        {
            this.dungeonGenerator = dungeonGenerator;
            this.monsterSpawner = monsterSpawner;
            this.itemSpawner = itemSpawner;
        }

        public GameState build(Random random)
        {
            Player player = new Player(random);
            GameState gameState = new GameState();
            gameState.player = player;
            this.generateNewDungeon(gameState);

            Console.WriteLine();

            return gameState;
        }

        public void generateNewDungeon(GameState gameState)
        {
            gameState.dungeon = this.dungeonGenerator.generate(this.dungeonLevel(gameState));
            gameState.player.dungeon = gameState.dungeon;
            gameState.player.move(gameState.dungeon.zones[0].startNode);
            

            gameState.player.numberOfCombatsOfDungeon = 0;

            this.monsterSpawner.spawnAssignment5(gameState.dungeon);
            this.itemSpawner.spawn(gameState.dungeon.zones, gameState.player.hitPoints);

        }

        private int dungeonLevel(GameState gameState)
        {
            if (gameState.dungeon == null) {
                return 1;
            }

            return gameState.dungeon.level + 1;
        }
    }
}
