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
        [TestMethod]
        public void PlayerMoveDown()
        {
            float expectedSpeed, expectedAngle, actualSpeed, actualAngle;
            int expectedFuel, actualFuel;

            Player subby = new Player() { Fuel = 100, Speed = 0, AngleDegrees = 0 };
            subby.GoDown();
            subby.GoDown();
            subby.GoDown();

            expectedAngle = 3;
            actualAngle = subby.AngleDegrees;

            expectedFuel = 97;
            actualFuel = subby.Fuel;

            expectedSpeed = 0;
            actualSpeed = subby.Speed;

            Assert.AreEqual(expectedAngle, actualAngle, "Angle niet goed wanneer subby omlaag roteert");
            Assert.AreEqual(expectedFuel, actualFuel, "Fuel niet goed wanneer subby omlaag roteert");
            Assert.AreEqual(expectedSpeed, actualSpeed, "Speed niet goed wanneer subby omlaag roteert");
        }
        [TestMethod]
        public void PlayerMoveUp()
        {
            float expectedSpeed, expectedAngle, actualSpeed, actualAngle;
            int expectedFuel, actualFuel;

            Player subby = new Player() { Fuel = 97, Speed = 0, AngleDegrees = 3 };
            subby.GoUp();

            expectedAngle = 170;
            actualAngle = subby.AngleDegrees;

            expectedFuel = 96;
            actualFuel = subby.Fuel;

            expectedSpeed = 0;
            actualSpeed = subby.Speed;

            Assert.AreEqual(expectedAngle, actualAngle,1, "Angle niet goed wanneer subby omhoog roteert");
            Assert.AreEqual(expectedFuel, actualFuel, "Fuel niet goed wanneer subby omhoog roteert");
            Assert.AreEqual(expectedSpeed, actualSpeed, "Speed niet goed wanneer subby omhoog roteert");
        }
        [TestMethod]
        public void PlayerGoFaster()
        {
            float expectedSpeed, expectedAngle, actualSpeed, actualAngle;
            int expectedFuel, actualFuel;

            Player subby = new Player() { Fuel = 96, Speed = 0, AngleDegrees = 2 };
            subby.GoFaster();
            subby.GoFaster();
            subby.GoFaster();
            subby.GoFaster();
            
            expectedAngle = 114;
            actualAngle = subby.AngleDegrees;

            expectedFuel = 88;
            actualFuel = subby.Fuel;

            expectedSpeed = 0.2f;
            actualSpeed = subby.Speed;

            Assert.AreEqual(expectedAngle, actualAngle,1, "Angle niet goed wanneer subby harder gaat");
            Assert.AreEqual(expectedFuel, actualFuel, "Fuel niet goed wanneer subby harder gaat");
            Assert.AreEqual(expectedSpeed, actualSpeed, "Speed niet goed wanneer subby harder gaat");
        }
        [TestMethod]
        public void PlayerGoSlower()
        {
            float expectedSpeed, expectedAngle, actualSpeed, actualAngle;
            int expectedFuel, actualFuel;

            Player subby = new Player() { Fuel = 88, Speed = 0.2f, AngleDegrees = 2 };

            subby.GoSlower();

            expectedAngle = 114;
            actualAngle = subby.AngleDegrees;

            expectedFuel = 86;
            actualFuel = subby.Fuel;

            expectedSpeed = 0.15f;
            actualSpeed = subby.Speed;

            Assert.AreEqual(expectedAngle, actualAngle,1, "Angle niet goed wanneer subby zachter gaat");
            Assert.AreEqual(expectedFuel, actualFuel, "Fuel niet goed wanneer subby zachter gaat");
            Assert.AreEqual(expectedSpeed, actualSpeed, "Speed niet goed wanneer subby zachter gaat");
        }

        [TestMethod]
        public void MaximumPlayerSpeed()
        {
            float expectedSpeed, actualSpeed;

            Player subby = new Player() { Fuel = 100000000, Speed = 0, Rotation = 0 };
            for (int i = 0; i < 10000; i++)
                subby.GoFaster();


            expectedSpeed = 4;
            actualSpeed = subby.Speed;


            Assert.AreEqual(expectedSpeed, actualSpeed, 0.1f, "Maximum speed is niet goed");
        }


        [TestMethod]
        public void IsSubbyAlive()
        {


            Boolean actual, expected;
            Player subby;
            Level level = new Level();

            subby = new Player() { Fuel = 100000000, Speed = 0, Health = 0 };
            level.Subby = subby;

            actual = level.IsSubbyAlive();
            expected = false;

            Assert.AreEqual(expected, actual, "Subby hoort dood te zijn bij 0 health");

            subby = new Player() { Fuel = 0, Speed = 0, Health = 10 };
            level.Subby = subby;

            actual = level.IsSubbyAlive();
            expected = false;

            Assert.AreEqual(expected, actual, "Subby hoort dood te zijn bij 0 fuel en 0 speed");


            subby = new Player() { Fuel = 0, Speed = 4, Health = 10 };
            level.Subby = subby;

            actual = level.IsSubbyAlive();
            expected = true;

            Assert.AreEqual(expected, actual, "Subby hoort levend te zijn bij 0 fuel en 4 speed, zolang hij nog snelheid heeft");

            subby = new Player() { Fuel = 1, Speed = 0, Health = 1 };
            level.Subby = subby;

            actual = level.IsSubbyAlive();
            expected = true;

            Assert.AreEqual(expected, actual, "Subby hoort levend te zijn bij 1 fuel en 1 health");
        }

        [TestMethod]
        public void PlayerUpdatePostion()
        {
            float expectedSpeed, expectedAngle, expectedX, expectedY, actualSpeed, actualAngle, actualX, actualY;
            int expectedFuel, actualFuel;

            Player subby = new Player() { Fuel = 96, Speed = 0, AngleDegrees = 0, Position = new Vector2(0, 0) };
            subby.GoFaster();
            subby.GoFaster();
            subby.GoFaster();
            subby.GoFaster();
            subby.GoFaster();
            subby.GoFaster();
            subby.GoFaster();
            subby.GoFaster();
            subby.UpdatePosition();

            expectedAngle = 0;
            actualAngle = subby.AngleDegrees;

            expectedFuel = 80;
            actualFuel = subby.Fuel;

            expectedSpeed = 0.39f;
            actualSpeed = subby.Speed;

            expectedX = 0.39f;
            actualX = subby.Position.X;

            expectedY = 0.9f;
            actualY = subby.Position.Y;

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

            Player subby = new Player() { Bullits = 10, AngleDegrees = 0, Position = new Vector2(0,0)};
            Level level = new Level();
            level.Subby = subby;
            level.MissileList = new List<Missile>();
            level.SpriteList = new List<ISprite>();
            Missile missile = level.Subby.Shoot();
            Point position = level.PointOnCircle(285 / 2 + 30, (int)level.Subby.AngleDegrees, new Point((int)level.Subby.Position.X, (int)level.Subby.Position.Y));
            level.CreateMissile(missile, position, 300);


            expectedBullits = 9;
            actualBullits = subby.Bullits;

            expectedPosition = new Vector2(172, 0);
            actualPosition = missile.Position;


            Assert.AreEqual(expectedBullits, actualBullits, "Er gaat geen kogel af wanneer er geschoten wordt");
            Assert.AreEqual(expectedPosition, actualPosition, "De kogel komt niet goed voor de player te staan.");
        }
    }
}
