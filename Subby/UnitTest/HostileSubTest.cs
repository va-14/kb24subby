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
        Vector2 expectedPosition, actualPosition, expectedVelocity, actualVelocity;
        float expectedRotation, actualRotation;
        private Player player = new Player() { Position = new Vector2(100, 800) };
        HostileSub sub = new HostileSub()
        {
            Position = new Vector2(0, 0),
            ShootTimer = 0,
            Rotation = 0,
            ScrollingPosition = 0,
            Boundaries = new GameBoundaries() { Bottom = 1000, Left = 20, Right = 800, Top = 300 },
            Velocity = new Vector2(0, 0)
        };
        GameTime gameTime = new GameTime();

        [TestMethod]
        public void HostileSubUpdate()
        {
            sub.Subby = player;
            sub.Strategy = new AimedShots();

            sub.Position = new Vector2(500, 800);
            sub.Velocity = new Vector2(4, 0);

            sub.Update(gameTime);

            expectedPosition = new Vector2(504, 800);
            actualPosition = sub.Position;
            expectedVelocity = new Vector2(4, 0);
            actualVelocity = sub.Velocity;
            expectedRotation = 0;
            actualRotation = sub.AngleDeg;

            Assert.AreEqual(expectedPosition, actualPosition, "Positie niet goed bij AimedShots strategie");
            Assert.AreEqual(expectedVelocity, actualVelocity, "Velocitie niet goed bij AimedShots strategie");
            Assert.AreEqual(expectedRotation, actualRotation, "Rotation niet goed bij AimedShots strategie");

            sub.Position = new Vector2(1010,900);
            sub.Velocity = new Vector2(4, 0);

            sub.Update(gameTime);

            expectedPosition = new Vector2(1006, 900);
            actualPosition = sub.Position;
            expectedVelocity = new Vector2(-4, 0);
            actualVelocity = sub.Velocity;
            expectedRotation = 0.1099303f;
            actualRotation = sub.Rotation;

            Assert.AreEqual(expectedPosition, actualPosition, "Positie niet goed bij AimedShots strategie");
            Assert.AreEqual(expectedVelocity, actualVelocity, "Velocitie niet goed bij AimedShots strategie");
            Assert.AreEqual(expectedRotation, actualRotation, "Rotation niet goed bij AimedShots strategie");

            sub.Strategy = new WallOfShots();

            sub.Position = new Vector2(2000, 800);
            sub.Velocity = new Vector2(4, 0);

            sub.Update(gameTime);

            expectedPosition = new Vector2(2000, 804);
            actualPosition = sub.Position;
            expectedVelocity = new Vector2(0, 4);
            actualVelocity = sub.Velocity;
            expectedRotation = 0;
            actualRotation = sub.AngleDeg;

            Assert.AreEqual(expectedPosition, actualPosition, "Positie niet goed bij WallOfShots strategie");
            Assert.AreEqual(expectedVelocity, actualVelocity, "Velocitie niet goed bij WallOfShots strategie");
            Assert.AreEqual(expectedRotation, actualRotation, "Rotation niet goed bij WallOfShots strategie");

            sub.Position = new Vector2(499, 800);
            sub.Velocity = new Vector2(-4, -4);

            sub.Update(gameTime);

            expectedPosition = new Vector2(503, 796);
            actualPosition = sub.Position;
            expectedVelocity = new Vector2(4, -4);
            actualVelocity = sub.Velocity;
            expectedRotation = 0;
            actualRotation = sub.AngleDeg;

            Assert.AreEqual(expectedPosition, actualPosition, "Positie niet goed bij WallOfShots strategie");
            Assert.AreEqual(expectedVelocity, actualVelocity, "Velocitie niet goed bij WallOfShots strategie");
            Assert.AreEqual(expectedRotation, actualRotation, "Rotation niet goed bij WallOfShots strategie");
        }

        [TestMethod]
        public void HostileSubShoot()
        {
            sub.Strategy = new AimedShots();
            sub.ShootTimer = -1;

            Assert.IsTrue(sub.Shoot(gameTime));
            Assert.AreEqual(2, sub.ShootTimer, "Shoot werkt niet goed bij AimedShots strategie");

            sub.ShootTimer = 2;

            Assert.IsFalse(sub.Shoot(gameTime));
            Assert.AreEqual(2, sub.ShootTimer, "Shoot werkt niet goed bij AimedShots strategie");

            sub.Strategy = new WallOfShots();
            sub.ShootTimer = -1;

            Assert.IsTrue(sub.Shoot(gameTime));
            Assert.AreEqual(-0.5f, sub.ShootTimer, "Shoot werkt niet goed bij WallOfShots strategie");

            sub.ShootTimer = 2;

            Assert.IsFalse(sub.Shoot(gameTime));
            Assert.AreEqual(2, sub.ShootTimer, "Shoot werkt niet goed bij WallOfShots strategie");
        }

        [TestMethod]
        public void HostileSubMoveLeft()
        {
            sub.Velocity = new Vector2(2, 2);
            expectedVelocity = new Vector2(-4, 2);

            sub.MoveLeft();

            Assert.AreEqual(expectedVelocity, sub.Velocity, "MoveLeft werkt niet goed");
        }

        [TestMethod]
        public void HostileSubMoveRight()
        {
            sub.Velocity = new Vector2(2, 2);
            expectedVelocity = new Vector2(4, 2);

            sub.MoveRight();

            Assert.AreEqual(expectedVelocity, sub.Velocity, "MoveRight werkt niet goed");
        }

        [TestMethod]
        public void HostileSubMoveUp()
        {
            sub.Velocity = new Vector2(2, 2);
            expectedVelocity = new Vector2(2, -4);

            sub.MoveUp();

            Assert.AreEqual(expectedVelocity, sub.Velocity, "MoveUp werkt niet goed");
        }

        [TestMethod]
        public void HostileSubMoveDown()
        {
            sub.Velocity = new Vector2(2, 2);
            expectedVelocity = new Vector2(2, 4);

            sub.MoveDown();

            Assert.AreEqual(expectedVelocity, sub.Velocity, "MoveDown werkt niet goed");
        }

        [TestMethod]
        public void HostileSubStopMoveSideways()
        {
            sub.Velocity = new Vector2(2, 2);
            expectedVelocity = new Vector2(0, 2);

            sub.StopMoveSideways();

            Assert.AreEqual(expectedVelocity, sub.Velocity, "StopMoveSideways werkt niet goed");
        }

        [TestMethod]
        public void HostileSubCollisionWith()
        {
            sub.Health = 400;
            Missile m = new Missile()
            {
                Damage = 200
            };
            sub.CollisionWith(m);

            Assert.AreEqual(200, sub.Health, "CollisionWith werkt niet goed");

            sub.Health = 400;

            sub.CollisionWith(new Mine());

            Assert.AreEqual(400, sub.Health, "CollisionWith werkt niet goed");
        }
    }
}
