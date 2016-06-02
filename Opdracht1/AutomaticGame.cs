using System;
using Opdracht1;
using Rogue.DomainObjects;
using Rogue.Services;

namespace Rogue
{
    public class AutomaticGame : Game
    {
        private int counter;
        
        public AutomaticGame(GameSerializer gameSerializer, GameBuilder gameBuilder,Random random) 
            : base(gameSerializer,gameBuilder,random) { }

        public override void turn()
        {
            TurnBot turn = new TurnBot(this);
            turn.doTurnPlayer(this.counter);
            turn.checkIfCombat();
            if (turn.checkNode())
            {
                turn = new TurnBot(this);
                turn.doTurnPacks();
                turn.checkIfCombat();
            }
            if (this.gameState.player.currentNode.zone != null && 
                this.gameState.player.currentNode == this.gameState.player.currentNode.zone.endNode)
            {
                this.counter = 0;
            }
            this.counter++;
        }

        public void useTimeCrystalOrNot()
        {
            foreach (Item item in this.gameState.player.bag)
            {
                if (item.GetType() == typeof(TimeCrystal))
                {
                    int timeCrystal = 1;//this.random.Next(0, 4);
                    if (timeCrystal == 1)
                    {
                        //this.player.useTimeCrystal(false, (TimeCrystal)item);
                        break;
                    }

                }
            }
        }
    }
}
