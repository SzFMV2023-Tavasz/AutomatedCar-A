namespace Tests.Models
{
    using AutomatedCar.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Xunit;

    public class Vector2DTest
    {
        [Fact]
        public void TestMagnitude()
        {
            Vector2D v = new Vector2D(3, 4);
            Assert.Equal(v.Magnitude, 5);
        }

        [Fact]
        public void TestUnitVector()
        {
            Vector2D v = new Vector2D(3, 4);
            Vector2D unit = v.UnitVector;

            Assert.Equal(unit.X, 3.0f / 5.0f);
            Assert.Equal(unit.Y, 4.0f / 5.0f);
        }

        [Fact]
        public void TestDotProduct()
        {
            Vector2D v1 = new Vector2D(1, 2);
            Vector2D v2 = new Vector2D(3, 4);

            float result = Vector2D.DotProduct(v1, v2);

            Assert.Equal(result, 11);
        }

        [Fact]
        public void TestCrossProduct()
        {
            Vector2D v1 = new Vector2D(1, 2);
            Vector2D v2 = new Vector2D(3, 4);

            float result = Vector2D.CrossProduct(v1, v2);

            Assert.Equal(result, -2);
        }

        [Fact]
        public void TestAddition()
        {
            Vector2D v1 = new Vector2D(1, 2);
            Vector2D v2 = new Vector2D(3, 4);

            Vector2D result = v1 + v2;

            Assert.Equal(result.X, 4);
            Assert.Equal(result.Y, 6);
        }

        [Fact]
        public void TestSubtraction()
        {
            Vector2D v1 = new Vector2D(1, 2);
            Vector2D v2 = new Vector2D(3, 4);

            Vector2D result = v1 - v2;

            Assert.Equal(result.X, -2);
            Assert.Equal(result.Y, -2);
        }

        [Fact]
        public void TestScalarMultiplication()
        {
            Vector2D v = new Vector2D(1, 2);

            Vector2D result = v * 2;

            Assert.Equal(result.X, 2);
            Assert.Equal(result.Y, 4);
        }

        [Fact]
        public void TestScalarDivision()
        {
            Vector2D v = new Vector2D(2, 4);

            Vector2D result = v / 2;

            Assert.Equal(result.X, 1);
            Assert.Equal(result.Y, 2);
        }

        [Fact]
        public void TestRotation()
        {
            Vector2D v = new Vector2D(0, 1);

            v.Rotate(45);
            Assert.Equal(Math.Sqrt(2) / -2, v.X, 5);
            Assert.Equal(Math.Sqrt(2) / 2, v.Y, 5);

            v.Rotate(45);
            Assert.Equal(-1, v.X, 5);
            Assert.Equal(0, v.Y, 5);

            v.Rotate(45);
            Assert.Equal(Math.Sqrt(2) / -2, v.X, 5);
            Assert.Equal(Math.Sqrt(2) / -2, v.Y, 5);

            v.Rotate(45);
            Assert.Equal(0, v.X, 5);
            Assert.Equal(-1, v.Y, 5);

            v.Rotate(45);
            Assert.Equal(Math.Sqrt(2) / 2, v.X, 5);
            Assert.Equal(Math.Sqrt(2) / -2, v.Y, 5);

            v.Rotate(45);
            Assert.Equal(1, v.X, 5);
            Assert.Equal(0, v.Y, 5);

            v = new Vector2D(0, 1);
            v.Rotate(-45);
            Assert.Equal(Math.Sqrt(2) / 2, v.X, 5);
            Assert.Equal(Math.Sqrt(2) / 2, v.Y, 5);
        }

        [Theory]
        [InlineData(90, 0, 1)]
        public void TestAngle(int expected, int x, int y)
        {
            Assert.Equal(expected, (new Vector2D(x, y)).Angle());
        }


    }

}
