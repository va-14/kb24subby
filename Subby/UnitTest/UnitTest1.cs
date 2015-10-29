using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Subby.Sprites;

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
    }
}
