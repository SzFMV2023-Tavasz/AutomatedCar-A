// <copyright file="GeometryUtils.cs" company="Team-A2">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AutomatedCar.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using AutomatedCar.Models;
    using Avalonia;

    /// <summary>
    /// Class responsible for frequently used geometric functions.
    /// </summary>
    public static class GeometryUtils
    {
        /// <summary>
        /// Transforms a point by rotating it around the rotationpoint and offsetting it to the position of its container worldObject.
        /// </summary>
        /// <param name="point">The point of the worldobject's polyline geometry, that we want to transform into its correct position. </param>
        /// <param name="worldObject">The world object in question. "point" is rotated around the world object's rotationpoint.</param>
        /// <returns>Returns the transformed (rotated and translated) point.</returns>
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

        /// <summary>
        /// Calculates the euclidian distance between the given points. The parameters are named "point" and "rotationPoint",
        /// because it is the most common use-case of the method, but can be used otherwise liberally.
        /// </summary>
        /// <param name="point">Point A.</param>
        /// <param name="rotationPoint">Rotation point or simply Point B.</param>
        /// <returns>Returns the euclidian distance between the given Point parameters.</returns>
        public static double GetEuclidianDistance(Point point, Point rotationPoint) => Math.Sqrt(Math.Pow(point.X - rotationPoint.X, 2) + Math.Pow(point.Y - rotationPoint.Y, 2));

        // ALT way, might reconsider later, also float might be sufficient, double is used primarily for consistency with Avalonia Points double coordinates
        /*private float CalculateDistance(Point transformedPointInWorld, Point carAnchorPointInWorld)
        {
            var vector = new Vector2((float)(transformedPointInWorld.X - carAnchorPointInWorld.X), (float)(transformedPointInWorld.Y - carAnchorPointInWorld.Y));

            var length = vector.Length();

            return length;
        }*/

        /// <summary>
        /// Converts the normal trigonometric angles (0° faces to the RIGHT, positive direction going COUNTER-CLOCKWISE)
        /// to the locally used rotation references seen in worldobjects (0° faces UPWARDS, positive direction going CLOCKWISE).
        /// </summary>
        /// <param name="degree">The "trigonometrically correct" degree to be converted.</param>
        /// <returns>Returns the angle that is in accordance with the locally used reference system seen in world objects' Rotation parameter.</returns>
        public static double GetRotation(double degree) => (-degree) + 90;

        /// <summary>
        /// Degree to radian converter.
        /// </summary>
        /// <param name="degree">Degree to convert, eg.: 180°.</param>
        /// <returns>Radian value of given degree.</returns>
        public static double DegToRad(double degree) => (Math.PI / 180) * degree;

        /// <summary>
        /// Radian to degree converter.
        /// </summary>
        /// <param name="radian">Radian to convert, eg.: PI.</param>
        /// <returns>Degree conversion of given radian.</returns>
        public static double RadToDeg(double radian) => (180 / Math.PI) * radian;

        /// <summary>
        /// Calculates the angle between the given points. The parameters are named "point" and "rotationPoint",
        /// because it is the most common use-case of the method, but can be used otherwise liberally.
        /// </summary>
        /// <param name="point">Point in euclidian space.</param>
        /// <param name="rotationPoint">Reference point.</param>
        /// <returns>Returns the angle enclosed between "point" and "rotationPoint" relative to "rotationPoint".</returns>
        public static double GetAngle(Point point, Point rotationPoint) => Math.Atan2(rotationPoint.Y - point.Y, point.X - rotationPoint.X);
    }
}
