using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Subby.Sprites;
using Microsoft.Xna.Framework;
using Subby;
using System.Collections.Generic;

namespace UnitTest
{
    [TestClass]
    public class MineTest
    {
        Mine mine;
        Player player;

        [TestInitialize]
        public void Setup()
        {
            mine = new Mine()
            {
                Position = new Vector2(50, 50),
                Damage = 200,
                Delay = 2,
                Exploded = true,
                Range = 20,
                _timeSinceActivated = 2.2f
            };
            player = new Player() 
            { 
                Position = new Vector2(50, 60),
                Health = 400
            };
        }

        [TestMethod]
        public void DoesDamageToPlayer()
        {
            mine.CollisionWith(player);

            Assert.AreEqual(200, player.Health);
        }
    }
}
