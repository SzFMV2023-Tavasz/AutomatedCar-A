﻿namespace AutomatedCar.SystemComponents
{
    using AutomatedCar.Models;
    using AutomatedCar.SystemComponents.Packets;
    using Avalonia;
    using Avalonia.Media;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class CollisionSensor : SystemComponent
    {
        public delegate void Collided(DetectedObjectInfo detectedO);
        public event Collided CollidedWNPCEvent;
        public event Collided CollidedWBuildingsEvent;
        public AutomatedCar car {get; }
        private IReadOnlyPacket<DetectedObjectInfo> Packet {get; }
        private List<WorldObjectType> WorldObjectTypesFilter = new List<WorldObjectType>() { WorldObjectType.Building,WorldObjectType.Car,WorldObjectType.Pedestrian,WorldObjectType.RoadSign,WorldObjectType.Tree, WorldObjectType.Other};
        public CollisionSensor(VirtualFunctionBus virtualFunctionBus, AutomatedCar car) : base(virtualFunctionBus)
        {
            this.car = car;
            Packet = new ReadOnlyPacket();
            virtualFunctionBus.CollisionPacket = this.Packet;
        }

        public override void Process()
        {
            Packet.WorldObjectsDetected = null;
            DetectWithCar();
        }

        private void DetectWithCar()
        {
            List<DetectedObjectInfo> detectedObjects = new List<DetectedObjectInfo>();
            List<Point> CarPoints = new List<Point>();

            Point rotationPoint = new Avalonia.Point(car.RotationPoint.X, car.RotationPoint.Y);
            foreach (var point in car.Geometry.Points)
            {
                double distance = GetEuclidianDistance(point, rotationPoint);
                double phi = GetAngle(point, rotationPoint) + DegToRad(-car.Rotation);
                Point transformedPoint = new Point(
                    (Math.Cos(phi) * distance) + car.X,
                    (-Math.Sin(phi) * distance) + car.Y);
                CarPoints.Add(transformedPoint);
            }
            PolylineGeometry CarLines = new PolylineGeometry(CarPoints, false);

            var collidableWorldObjects = World.Instance.WorldObjects.Where(obj => this.WorldObjectTypesFilter.Contains(obj.WorldObjectType) && !obj.Equals(this.car));
            foreach (var worldObject in collidableWorldObjects)
            {
                Point worldObjectRotationPoint = new Avalonia.Point(worldObject.RotationPoint.X, worldObject.RotationPoint.Y);
                if (worldObject.Geometries.Count > 0)
                {
                    foreach (var point in worldObject.Geometries[0].Points)
                    {
                        // Every boundary boxes at the origo, so needs to be transformed at its position
                        double distance = GetEuclidianDistance(point, worldObjectRotationPoint);
                        double phi = GetAngle(point, worldObjectRotationPoint) + DegToRad(-worldObject.Rotation);
                        Point transformedPoint = new Point(
                            (Math.Cos(phi) * distance) + worldObject.X,
                            (-Math.Sin(phi) * distance) + worldObject.Y);

                        bool detected = CarLines.FillContains(transformedPoint);

                        if (detected)
                        {
                            var seged = new DetectedObjectInfo()
                            {
                                DetectedObject = worldObject,
                                Distance = 0
                            };
                            Packet.WorldObjectsDetected = new List<DetectedObjectInfo>() { seged };
                            if (worldObject.WorldObjectType == WorldObjectType.Pedestrian)
                            {
                                CollidedWNPCEvent?.Invoke(seged);
                            }
                            if (worldObject.WorldObjectType == WorldObjectType.Building)
                            {
                                CollidedWBuildingsEvent?.Invoke(seged);
                            }
                            return;
                        }

                    }
                }
            }
        }

        private static double GetEuclidianDistance(Point point, Point rotationPoint) => Math.Sqrt(Math.Pow(point.X - rotationPoint.X, 2) + Math.Pow(point.Y - rotationPoint.Y, 2));

        private static double DegToRad(double degree) => (Math.PI / 180) * degree;

        private static double GetAngle(Point point, Point rotationPoint) => Math.Atan2(rotationPoint.Y - point.Y, point.X - rotationPoint.X);
    }
}
