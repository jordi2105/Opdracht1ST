using System;
using Rogue.DomainObjects;

namespace Rogue
{
    [Serializable]
    public class GameState
    {
        public Dungeon dungeon { get; set; }
        public Player player { get; set; }
        public bool isAlive { get; set; }
        public bool turnPlayer { get; set; }
        public int teller { get; set; }

        public GameState(Random random)
        {
            this.teller = 0;
            this.player = new Player(random);
            this.isAlive = true;
            this.turnPlayer = true;
        }
    }
}