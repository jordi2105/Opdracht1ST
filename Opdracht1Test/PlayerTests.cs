using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Opdracht1.Tests
{
    [TestClass()]
    public class PlayerTests
    {
        [TestMethod]
        public void healing_potion_heals_player()
        {
            Player player = new Player();
            player.hitPoints = 10;
            player.getCommand("use-potion", null, new HealingPotion(45), false);
            Assert.AreEqual(55, player.hitPoints); 
        }

        [TestMethod]
        public void healing_potion_does_make_player_hp_exceed_max()
        {
            Player player = new Player();
            player.hitPoints = 90;
            player.getCommand("use-potion", null, new HealingPotion(45), false);
            Assert.AreEqual(100, player.hitPoints); 
        }
    }
}