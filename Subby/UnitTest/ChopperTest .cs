using System;
using Microsoft.Xna.Framework;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Subby.Sprites;
using Subby;
using System.Runtime.Serialization;
using System.Data;
using System.Runtime;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;


namespace UnitTest
{
    [TestClass]
    public class ChopperTest
    {
        Level level;
        Missile missile;
        Chopper chopper;

        [TestInitialize]
        public void setup()
        {
            missile = new Missile()
            {
                Damage = 300,
                Active = true,
                Speed = 3f,
                Exploded = false,
                Position = new Vector2(0, 0)
            };
            chopper = new Chopper()
            {
                Health = 300,
                Speed = 2f,
                Position = new Vector2 (20,20)
            };
            
            level = new Level();
        }
        [TestMethod]
        public void CollisionWithMissileTrue()
        {
            Boolean collision = false;

            Rectangle missileRect = new Rectangle((int)missile.Position.X, (int)missile.Position.Y, 50, 50);
            Rectangle chopperRect = new Rectangle((int)chopper.Position.X, (int)chopper.Position.Y, 50, 50);

            Rectangle overlap = Rectangle.Intersect(missileRect, chopperRect);

            if (!overlap.IsEmpty)
            {
                collision = true;
            }
            Assert.AreEqual(true, collision, "Er vindt geen collision plaats ");
        }
        [TestMethod]
        public void CollisionWithMissileFalse()
        {
            Boolean collision = false;

            Rectangle missileRect = new Rectangle((int)missile.Position.X, (int)missile.Position.Y, 50, 50);
            Rectangle chopperRect = new Rectangle((int)chopper.Position.X + 70, (int)chopper.Position.Y, 50, 50);

            Rectangle overlap = Rectangle.Intersect(missileRect, chopperRect);

            if (!overlap.IsEmpty)
            {
                collision = true;
            }
            Assert.AreEqual(false, collision, "Er vindt wel collision plaats");
        }

        [TestMethod]
        public void CollisionWithMissileHealth()
        {
            int healthChopper;

            chopper.CollisionWith(missile);

            healthChopper = chopper.Health;

            Assert.AreEqual(0, healthChopper, "Er gaat iets mis met de health en kogel collision");
        }
    }
}
