using System;
using Rogue.DomainObjects;
using Rogue.Services;

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

        /*public GameState(Player player)
        {
            this.teller = 0;
            this.player = player;
            this.isAlive = true;
            this.turnPlayer = true;
        }*/
        public GameState()
        {
            this.teller = 0;
            this.isAlive = true;
            this.turnPlayer = true;
        }
    }
}