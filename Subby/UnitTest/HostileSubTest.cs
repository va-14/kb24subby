using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Subby.Sprites;
using Microsoft.Xna.Framework;
using Subby.Strategies;
using Subby;

namespace UnitTest
{
    [TestClass]
    public class HostileSubTest
    {
        Vector2 expectedPosition, expectedVelocity;
        float expectedRotation;
        Player player;
        HostileSub sub;
        GameTime gameTime;

        [TestInitialize]
        public void Setup()
        {
            player = new Player() { Position = new Vector2(100, 800) };
            sub = new HostileSub()
            {
                Position = new Vector2(800, 500),
                ShootTimer = 0,
                Rotation = 0,
                ScrollingPosition = 0,
                Boundaries = new LevelBoundaries() { Bottom = 1000, Left = 20, Right = 800, Top = 300 },
                Velocity = new Vector2(-4, 0),
                Subby = player,
                Health = 400,
                Strategy = new AimedShots(),
                Active = true
            };
            gameTime = new GameTime();
        }

        [TestMethod]
        public void DoNotMoveWhileInactive()
        {
            sub.Active = false;
            sub.Update(gameTime);
            expectedPosition = new Vector2(800, 500);

            Assert.AreEqual(expectedPosition, sub.Position);
        }

        [TestMethod]
        public void MoveWhileActive()
        {
            sub.Active = true;
            sub.Update(gameTime);
            expectedPosition = new Vector2(796, 500);

            Assert.AreEqual(expectedPosition, sub.Position);
        }

        [TestMethod]
        public void MoveWithAimedShotsStrategy()
        {
            sub.Strategy = new AimedShots();
            sub.Update(gameTime);
            expectedPosition = new Vector2(796, 500);
            expectedRotation = -0.4069708f;

            Assert.AreEqual(expectedPosition, sub.Position);
            Assert.AreEqual(expectedRotation, sub.Rotation);
        }

        [TestMethod]
        public void MoveWithWallOfShotsStrategy()
        {
            sub.Strategy = new WallOfShots();
            sub.Update(gameTime);
            expectedPosition = new Vector2(800, 504);
            expectedRotation = 0;

            Assert.AreEqual(expectedPosition, sub.Position);
            Assert.AreEqual(expectedRotation, sub.Rotation);
        }

        [TestMethod]
        public void ShootWithAimedShotsStrategy()
        {
            sub.Strategy = new AimedShots();
            sub.Shoot(2);

            Assert.AreEqual(3, sub.ShootTimer);

            sub.ShootTimer = 5;
            sub.Shoot(2);

            Assert.AreEqual(5, sub.ShootTimer);
        }

        [TestMethod]
        public void ShootWithWallOfShotsStrategy()
        {
            sub.Strategy = new WallOfShots();
            sub.Shoot(2);

            Assert.AreEqual(0.5f, sub.ShootTimer);

            sub.ShootTimer = 5;
            sub.Shoot(2);

            Assert.AreEqual(5, sub.ShootTimer);
        }

        [TestMethod]
        public void MoveLeft()
        {
            sub.MoveLeft();
            expectedVelocity = new Vector2(-4, 0);            

            Assert.AreEqual(expectedVelocity, sub.Velocity);
        }

        [TestMethod]
        public void MoveRight()
        {
            sub.MoveRight();
            expectedVelocity = new Vector2(4, 0);            

            Assert.AreEqual(expectedVelocity, sub.Velocity);
        }

        [TestMethod]
        public void MoveUp()
        {
            sub.MoveUp();
            expectedVelocity = new Vector2(-4, -4);            

            Assert.AreEqual(expectedVelocity, sub.Velocity);
        }

        [TestMethod]
        public void MoveDown()
        {
            sub.MoveDown();
            expectedVelocity = new Vector2(-4, 4);            

            Assert.AreEqual(expectedVelocity, sub.Velocity);
        }

        [TestMethod]
        public void StopMoveSideways()
        {
            sub.StopMoveSideways();
            expectedVelocity = new Vector2(0, 0);            

            Assert.AreEqual(expectedVelocity, sub.Velocity);
        }

        [TestMethod]
        public void CollisionWithMissile()
        {
            Missile m = new Missile()
            {
                Damage = 200
            };
            sub.CollisionWith(m);

            Assert.AreEqual(200, sub.Health);
        }
    }
}
