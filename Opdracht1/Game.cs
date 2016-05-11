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
        

        public Game(DungeonGenerator dungeonGenerator, GameSerializer gameSerializer)
        {

            player = new Player();
            this.dungeonGenerator = dungeonGenerator;
<<<<<<< HEAD
            nextDungeon();
            player.move(dungeon.getZones()[0].getStartNode());
            //ItemSpawner itemSpawner
            
=======
            this.gameSerializer = gameSerializer;
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
>>>>>>> 1a73a0bbf93c78a20b340431e329f0c614380a1b
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
