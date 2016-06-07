using System;
using Rogue.DomainObjects;

namespace Rogue.Services
{
    public class GameBuilder
    {
        private readonly DungeonGenerator dungeonGenerator;
        private readonly MonsterSpawner monsterSpawner;
        private readonly ItemSpawner itemSpawner;

        public GameBuilder(DungeonGenerator dungeonGenerator, MonsterSpawner monsterSpawner, ItemSpawner itemSpawner)
        {
            this.dungeonGenerator = dungeonGenerator;
            this.monsterSpawner = monsterSpawner;
            this.itemSpawner = itemSpawner;
        }

        public GameState buildNewGameState(Random random)
        {

            GameState gameState = new GameState(random);
            this.generateNewDungeon(gameState);

            Console.WriteLine();
            Console.WriteLine("New dungeon has been generated");

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
