using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Subby.Sprites;
using Subby;

namespace UnitTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void PlayerMovings()
        {
            float expectedSpeed, expectedAngle, actualSpeed, actualAngle;
            int expectedFuel, actualFuel;

            Player subby = new Player() { Fuel = 100, Speed = 0, Rotation = 0 };
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

            subby.GoUp();

            expectedAngle = 2;
            actualAngle = subby.AngleDegrees;

            expectedFuel = 96;
            actualFuel = subby.Fuel;

            expectedSpeed = 0;
            actualSpeed = subby.Speed;

            Assert.AreEqual(expectedAngle, actualAngle, "Angle niet goed wanneer subby omhoog roteert");
            Assert.AreEqual(expectedFuel, actualFuel, "Fuel niet goed wanneer subby omhoog roteert");
            Assert.AreEqual(expectedSpeed, actualSpeed, "Speed niet goed wanneer subby omhoog roteert");

            subby.GoFaster();
            subby.GoFaster();
            subby.GoFaster();
            subby.GoFaster();

            expectedAngle = 2;
            actualAngle = subby.AngleDegrees;

            expectedFuel = 88;
            actualFuel = subby.Fuel;

            expectedSpeed = 0.2f;
            actualSpeed = subby.Speed;

            Assert.AreEqual(expectedAngle, actualAngle, "Angle niet goed wanneer subby harder gaat");
            Assert.AreEqual(expectedFuel, actualFuel, "Fuel niet goed wanneer subby harder gaat");
            Assert.AreEqual(expectedSpeed, actualSpeed, "Speed niet goed wanneer subby harder gaat");


            subby.GoSlower();

            expectedAngle = 2;
            actualAngle = subby.AngleDegrees;

            expectedFuel = 86;
            actualFuel = subby.Fuel;

            expectedSpeed = 0.15f;
            actualSpeed = subby.Speed;

            Assert.AreEqual(expectedAngle, actualAngle, "Angle niet goed wanneer subby zachter gaat");
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
        public void GameReset()
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


    }
}
