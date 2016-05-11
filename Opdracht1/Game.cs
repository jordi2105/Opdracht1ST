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
        [NonSerialized] private readonly DungeonGenerator dungeonGenerator;
        [NonSerialized] private readonly GameSerializer gameSerializer;
        [NonSerialized] private readonly MonsterSpawner monsterSpawner;
        [NonSerialized] private readonly ItemSpawner itemSpawner;

        private Dungeon dungeon;
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
            this.itemSpawner.spawn(this.dungeon.zones, this.player.HitPoints);
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
