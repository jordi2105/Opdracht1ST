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

        public GameState()
        {
            this.teller = 0;
            this.player = new Player();;
            this.isAlive = true;
            this.turnPlayer = true;
        }
    }
}