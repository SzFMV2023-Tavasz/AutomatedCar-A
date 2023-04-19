namespace Tests.Models
{
    using AutomatedCar.Models;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    [TestClass]
    public class Vector2DTest
    {
        [TestMethod]
        public void TestMagnitude()
        {
            Vector2D v = new Vector2D(3, 4);
            Assert.AreEqual(v.Magnitude, 5);
        }

        [TestMethod]
        public void TestUnitVector()
        {
            Vector2D v = new Vector2D(3, 4);
            Vector2D unit = v.UnitVector;

            Assert.AreEqual(unit.X, 3.0f / 5.0f);
            Assert.AreEqual(unit.Y, 4.0f / 5.0f);
        }

        [TestMethod]
        public void TestDotProduct()
        {
            Vector2D v1 = new Vector2D(1, 2);
            Vector2D v2 = new Vector2D(3, 4);

            float result = Vector2D.DotProduct(v1, v2);

            Assert.AreEqual(result, 11);
        }

        [TestMethod]
        public void TestCrossProduct()
        {
            Vector2D v1 = new Vector2D(1, 2);
            Vector2D v2 = new Vector2D(3, 4);

            float result = Vector2D.CrossProduct(v1, v2);

            Assert.AreEqual(result, -2);
        }

        [TestMethod]
        public void TestAddition()
        {
            Vector2D v1 = new Vector2D(1, 2);
            Vector2D v2 = new Vector2D(3, 4);

            Vector2D result = v1 + v2;

            Assert.AreEqual(result.X, 4);
            Assert.AreEqual(result.Y, 6);
        }

        [TestMethod]
        public void TestSubtraction()
        {
            Vector2D v1 = new Vector2D(1, 2);
            Vector2D v2 = new Vector2D(3, 4);

            Vector2D result = v1 - v2;

            Assert.AreEqual(result.X, -2);
            Assert.AreEqual(result.Y, -2);
        }

        [TestMethod]
        public void TestScalarMultiplication()
        {
            Vector2D v = new Vector2D(1, 2);

            Vector2D result = v * 2;

            Assert.AreEqual(result.X, 2);
            Assert.AreEqual(result.Y, 4);
        }

        [TestMethod]
        public void TestScalarDivision()
        {
            Vector2D v = new Vector2D(2, 4);

            Vector2D result = v / 2;

            Assert.AreEqual(result.X, 1);
            Assert.AreEqual(result.Y, 2);
        }
    }

}
