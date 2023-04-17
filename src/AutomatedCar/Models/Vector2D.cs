namespace AutomatedCar.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class Vector2D
    {
        public float X { get; set; } // Horizontal component
        public float Y { get; set; } // Vertical component

        public Vector2D(float x, float y)
        {
            this.X = x;
            this.Y = y;
        }

        public double Magnitude
        {
            get
            {
                return Math.Sqrt(Math.Pow(X, 2) + Math.Pow(Y, 2));
            }
        }

        public Vector2D UnitVector
        {
            get
            {
                float magnitude = (float)this.Magnitude;
                return new Vector2D(X / magnitude, Y / magnitude);
            }
        }

        public static float DotProduct(Vector2D v1, Vector2D v2)
        {
            return v1.X * v2.X + v1.Y * v2.Y;
        }

        public static float CrossProduct(Vector2D v1, Vector2D v2)
        {
            return v1.X * v2.Y - v1.Y * v2.X;
        }

        public static Vector2D operator +(Vector2D v1, Vector2D v2)
        {
            return new Vector2D(v1.X + v2.X, v1.Y + v2.Y);
        }

        public static Vector2D operator -(Vector2D v1, Vector2D v2)
        {
            return new Vector2D(v1.X - v2.X, v1.Y - v2.Y);
        }

        public static Vector2D operator *(Vector2D v1, float scalar)
        {
            return new Vector2D(v1.X * scalar, v1.Y * scalar);
        }

        public static Vector2D operator /(Vector2D v1, float scalar)
        {
            return new Vector2D(v1.X / scalar, v1.Y / scalar);
        }
    }

}
