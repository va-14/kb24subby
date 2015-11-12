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
    public class PlayerTest
    {
        Level level;
        Wrak wrak;

        [TestInitialize]
        public void setup()
        {
            Player subby;
            subby = new Player() { 
                Fuel = 1000, 
                Speed = 0, 
                AngleDegrees = 0,
                Position = new Vector2(0, 0), 
                 Bullits = 10,
                 Health = 1000
            };
            
            level = new Level();
            level.Subby = subby;

            wrak = new Wrak();
            wrak.Damage = 1;
            wrak.Position = new Vector2(0, 0);
        }
        [TestMethod]
        public void PlayerMoveDown()
        {
            float expectedSpeed, expectedAngle, actualSpeed, actualAngle;
            int expectedFuel, actualFuel;

            level.Subby.GoDown();
            level.Subby.GoDown();
            level.Subby.GoDown();

            expectedAngle = 3;
            actualAngle = level.Subby.AngleDegrees;

            expectedFuel = 997;
            actualFuel = level.Subby.Fuel;

            expectedSpeed = 0;
            actualSpeed = level.Subby.Speed;

            Assert.AreEqual(expectedAngle, actualAngle, "Angle niet goed wanneer subby omlaag roteert");
            Assert.AreEqual(expectedFuel, actualFuel, "Fuel niet goed wanneer subby omlaag roteert");
            Assert.AreEqual(expectedSpeed, actualSpeed, "Speed niet goed wanneer subby omlaag roteert");
        }
        [TestMethod]
        public void PlayerMoveUp()
        {
            float expectedSpeed, expectedAngle, actualSpeed, actualAngle;
            int expectedFuel, actualFuel;

            level.Subby.GoUp();

            expectedAngle = 359;
            actualAngle = level.Subby.AngleDegrees;

            expectedFuel = 999;
            actualFuel = level.Subby.Fuel;

            expectedSpeed = 0;
            actualSpeed = level.Subby.Speed;

            Assert.AreEqual(expectedAngle, actualAngle,1, "Angle niet goed wanneer subby omhoog roteert");
            Assert.AreEqual(expectedFuel, actualFuel, "Fuel niet goed wanneer subby omhoog roteert");
            Assert.AreEqual(expectedSpeed, actualSpeed, "Speed niet goed wanneer subby omhoog roteert");
        }
        [TestMethod]
        public void PlayerGoFaster()
        {
            float expectedSpeed, expectedAngle, actualSpeed, actualAngle;
            int expectedFuel, actualFuel;

            level.Subby.GoFaster();
            level.Subby.GoFaster();
            level.Subby.GoFaster();
            level.Subby.GoFaster();
            
            expectedAngle = 1;
            actualAngle = level.Subby.AngleDegrees;

            expectedFuel = 992;
            actualFuel = level.Subby.Fuel;

            expectedSpeed = 0.2f;
            actualSpeed = level.Subby.Speed;

            Assert.AreEqual(expectedAngle, actualAngle,1, "Angle niet goed wanneer subby harder gaat");
            Assert.AreEqual(expectedFuel, actualFuel, "Fuel niet goed wanneer subby harder gaat");
            Assert.AreEqual(expectedSpeed, actualSpeed, "Speed niet goed wanneer subby harder gaat");
        }
        [TestMethod]
        public void PlayerGoSlower()
        {
            float expectedSpeed, expectedAngle, actualSpeed, actualAngle;
            int expectedFuel, actualFuel;


            level.Subby.GoSlower();

            expectedAngle = 1;
            actualAngle = level.Subby.AngleDegrees;

            expectedFuel = 998;
            actualFuel = level.Subby.Fuel;

            expectedSpeed = -0.05f;
            actualSpeed = level.Subby.Speed;

            Assert.AreEqual(expectedAngle, actualAngle,1, "Angle niet goed wanneer subby zachter gaat");
            Assert.AreEqual(expectedFuel, actualFuel, "Fuel niet goed wanneer subby zachter gaat");
            Assert.AreEqual(expectedSpeed, actualSpeed, "Speed niet goed wanneer subby zachter gaat");
        }

        [TestMethod]
        public void MaximumPlayerSpeed()
        {
            float expectedSpeed, actualSpeed;

            for (int i = 0; i < 1000; i++)
                level.Subby.GoFaster();


            expectedSpeed = 4;
            actualSpeed = level.Subby.Speed;


            Assert.AreEqual(expectedSpeed, actualSpeed, 0.1f, "Maximum speed is niet goed");
        }


        [TestMethod]
        public void IsSubbyAlive()
        {


            Boolean actual, expected;
            level.Subby.Fuel = 10;
            level.Subby.Speed = 1;
            level.Subby.Health = 0;

            actual = level.IsSubbyAlive();
            expected = false;

            Assert.AreEqual(expected, actual, "Subby hoort dood te zijn bij 0 health");

            level.Subby.Fuel = 0;
            level.Subby.Speed = 0;
            level.Subby.Health =10;

            actual = level.IsSubbyAlive();
            expected = false;

            Assert.AreEqual(expected, actual, "Subby hoort dood te zijn bij 0 fuel en 0 speed");

            level.Subby.Fuel = 0;
            level.Subby.Speed = 4;
            level.Subby.Health = 10;

            actual = level.IsSubbyAlive();
            expected = true;

            Assert.AreEqual(expected, actual, "Subby hoort levend te zijn bij 0 fuel en 4 speed, zolang hij nog snelheid heeft");

            level.Subby.Fuel = 1;
            level.Subby.Speed = 0;
            level.Subby.Health = 1;

            actual = level.IsSubbyAlive();
            expected = true;

            Assert.AreEqual(expected, actual, "Subby hoort levend te zijn bij 1 fuel en 1 health");
        }

        [TestMethod]
        public void PlayerUpdatePostion()
        {
            float expectedSpeed, expectedAngle, expectedX, expectedY, actualSpeed, actualAngle, actualX, actualY;
            int expectedFuel, actualFuel;

            level.Subby.GoFaster();
            level.Subby.GoFaster();
            level.Subby.GoFaster();
            level.Subby.GoFaster();
            level.Subby.GoFaster();
            level.Subby.GoFaster();
            level.Subby.GoFaster();
            level.Subby.GoFaster();
            level.Subby.UpdatePosition();

            expectedAngle = 0;
            actualAngle = level.Subby.AngleDegrees;

            expectedFuel = 984;
            actualFuel = level.Subby.Fuel;

            expectedSpeed = 0.39f;
            actualSpeed = level.Subby.Speed;

            expectedX = 0.39f;
            actualX = level.Subby.Position.X;

            expectedY = 0.9f;
            actualY = level.Subby.Position.Y;

            Assert.AreEqual(expectedX, actualX,0.1f, "positionX niet goed wanneer de player position wordt geupdate");
            Assert.AreEqual(expectedY, actualY, 0.1f, "positionY niet goed wanneer de player position wordt geupdate");
            Assert.AreEqual(expectedAngle, actualAngle, "Angle niet goed wanneer de player position wordt geupdate");
            Assert.AreEqual(expectedFuel, actualFuel, "Fuel niet goed wanneer de player position wordt geupdate");
            Assert.AreEqual(expectedSpeed, actualSpeed, 0.1f, "Speed niet goed wanneer de player position wordt geupdate");
        }
        [TestMethod]
        public void Shoot()
        {
            float expectedBullits, actualBullits;
            Vector2 expectedPosition,actualPosition;

            level.MissileList = new List<Missile>();
            level.SpriteList = new List<ISprite>();
            Missile missile = level.Subby.Shoot();
            Point position = level.PointOnCircle(285 / 2 + 30, (int)level.Subby.AngleDegrees, new Point((int)level.Subby.Position.X, (int)level.Subby.Position.Y));
            level.CreateMissile(missile, position, 300);


            expectedBullits = 9;
            actualBullits = level.Subby.Bullits;

            expectedPosition = new Vector2(172, 0);
            actualPosition = missile.Position;


            Assert.AreEqual(expectedBullits, actualBullits, "Er gaat geen kogel af wanneer er geschoten wordt");
            Assert.AreEqual(expectedPosition, actualPosition, "De kogel komt niet goed voor de player te staan.");
        }

        [TestMethod]
        public void CollisionWithWrakTrue()
        {
            Boolean collision = false;

            Rectangle wrakRect = new Rectangle((int)wrak.Position.X, (int)wrak.Position.Y, 50, 50);
            List<Rectangle> subbyRects = level.CalculateSubbyRect(400, 400, new Point(20, 20), 30);

            foreach (Rectangle subbyRect in subbyRects)
            {
                Rectangle overlap = Rectangle.Intersect(wrakRect, subbyRect);

                if (!overlap.IsEmpty)
                {
                    collision = true;
                }
            }
            Assert.AreEqual(true, collision, "Er vindt geen collision plaats met het wrak");
        }
        [TestMethod]
        public void CollisionWithWrakFalse()
        {
            Boolean collision = false;

            Rectangle wrakRect = new Rectangle((int)wrak.Position.X, (int)wrak.Position.Y, 50, 50);
            List<Rectangle> subbyRects = level.CalculateSubbyRect(400, 400, new Point(350, 350), 30);

            foreach (Rectangle subbyRect in subbyRects)
            {
                Rectangle overlap = Rectangle.Intersect(wrakRect, subbyRect);

                if (!overlap.IsEmpty)
                {
                    collision = true;
                }
            }
            Assert.AreEqual(false, collision, "Er vindt wel collision plaats met het wrak");
        }
        [TestMethod]
        public void CollisionWithWrak()
        {
            int health;
            level.Subby.CollisionWith(wrak);

            health = level.Subby.Health;

            Assert.AreEqual(999, health, "De schade gaat niet van subby af");
        }

        [TestMethod]
        public void CollisionWithTankstation()
        {
            int tankSubby, tankTanksation;
            TankStation tankstation = new TankStation() { Tank = 1000 };

            level.Subby.CollisionWith(tankstation);

            tankSubby = level.Subby.Fuel;
            tankTanksation = tankstation.Tank;

            Assert.AreEqual(2000, tankSubby, "De fuel komt niet bij de player");
            Assert.AreEqual(0, tankTanksation, "De fuel gaat niet weg bij tankstation");
        }
        [TestMethod]
        public void CollisionWithMissile()
        {
            int healthSubby;
            Missile missile = new Missile() { Damage = 300 };

            level.Subby.CollisionWith(missile);

            healthSubby = level.Subby.Health;

            Assert.AreEqual(700, healthSubby, "Er gaat iets mis met de health en kogel collision");
        }
        [TestMethod]
        public void DamagedPlayer()
        {
            Vector2 damagedBehavour;
            level.Subby.Health = 6;

            damagedBehavour = level.Subby.GetDamagedPositionBehavour();

            Assert.AreEqual(new Vector2(0.9f, 0), damagedBehavour, "De damagedbehavour klopt niet");
        }

    }
}
