using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opdracht1
{
    class Game
    {
        private DungeonGenerator dungeonGenerator;

        private Player player;
        private Dungeon dungeon;
        private List<Pack> packs;
        private List<Item> items;
        

        public Game(DungeonGenerator dungeonGenerator)
        {

            player = new Player();
            this.dungeonGenerator = dungeonGenerator;
            nextDungeon();
            player.move(dungeon.getZones()[0].getStartNode());
            //ItemSpawner itemSpawner
            
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
