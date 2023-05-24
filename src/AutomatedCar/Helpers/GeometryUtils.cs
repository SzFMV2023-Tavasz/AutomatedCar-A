namespace AutomatedCar.Helpers
{
    using AutomatedCar.Models;
    using Avalonia;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public static class GeometryUtils
    {
        // Transforms a point by rotating it around the rotationpoint and offsetting it to the position of its container worldObject
        public static Point TransformPoint(Point point, WorldObject worldObject)
        {
            // this weird rotationPoint construction is necessary because of the Drawing.Point -> Avalonia.Point conversion
            Point rotationPoint = new Avalonia.Point(worldObject.RotationPoint.X, worldObject.RotationPoint.Y);
            double distance = GetEuclidianDistance(point, rotationPoint);
            double phi = GetAngle(point, rotationPoint) + DegToRad(-worldObject.Rotation);
            Point transformedPoint = new Point(
                (Math.Cos(phi) * distance) + worldObject.X,
                (-Math.Sin(phi) * distance) + worldObject.Y);

            return transformedPoint;
        }

        public static double GetEuclidianDistance(Point point, Point rotationPoint) => Math.Sqrt(Math.Pow(point.X - rotationPoint.X, 2) + Math.Pow(point.Y - rotationPoint.Y, 2));

        // ALT way, might reconsider later, also float might be sufficient, double is used primarily for consistency with Avalonia Points double coordinates
        /*private float CalculateDistance(Point transformedPointInWorld, Point carAnchorPointInWorld)
        {
            var vector = new Vector2((float)(transformedPointInWorld.X - carAnchorPointInWorld.X), (float)(transformedPointInWorld.Y - carAnchorPointInWorld.Y));

            var length = vector.Length();

            return length;
        }*/

        public static double DegToRad(double degree) => (Math.PI / 180) * degree;

        public static double RadToDeg(double radian) => (180 / Math.PI) * radian;

        public static double GetAngle(Point point, Point rotationPoint) => Math.Atan2(rotationPoint.Y - point.Y, point.X - rotationPoint.X);
    }
}
