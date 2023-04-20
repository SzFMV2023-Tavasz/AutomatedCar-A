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

        public void Rotate(double angleOfRotation)
        {
            double originalAngle = this.Angle();
            double sqrt = Math.Sqrt(Math.Pow(this.X, 2) + Math.Pow(this.Y, 2));
            double sumOfAngles = originalAngle + angleOfRotation;
            double cos = Math.Cos(this.DegToRad(sumOfAngles));
            double sin = Math.Sin(this.DegToRad(sumOfAngles));

            double newX = sqrt * cos;
            double newY = sqrt * sin;

            this.X = (float)Math.Round(newX, 15);
            this.Y = (float)Math.Round(newY, 15);
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

        public double Angle()
        {
            double counter = (this.X * 1) + (this.Y * 0);
            double denominator = Math.Sqrt(Math.Pow(this.X, 2) + Math.Pow(this.Y, 2));
            double multiplier = this.Y / Math.Abs(this.Y);
            double ratio = counter / denominator;
            double acos = Math.Acos(ratio);

            return this.RadToDeg(multiplier * acos);
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

        private double RadToDeg(double rad)
        {
            return Math.Round(rad * 180 / Math.PI, 5);
        }

        private double DegToRad(double deg)
        {
            return deg / 180 * Math.PI;
        }
    }

}
