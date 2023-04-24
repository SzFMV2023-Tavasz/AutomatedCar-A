namespace AutomatedCar.SystemComponents
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
    using AutomatedCar.Helpers;

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

            foreach (var point in car.Geometry.Points)
            {
                Point transformedPoint = GeometryUtils.TransformPoint(point, car);
                CarPoints.Add(transformedPoint);
            }
            PolylineGeometry CarLines = new PolylineGeometry(CarPoints, false);

            var collidableWorldObjects = World.Instance.WorldObjects.Where(obj => this.WorldObjectTypesFilter.Contains(obj.WorldObjectType) && !obj.Equals(this.car));
            foreach (var worldObject in collidableWorldObjects)
            {
                foreach (var geometry in worldObject.Geometries)
                {
                    foreach (var point in geometry.Points)
                    {
                        Point transformedPoint = GeometryUtils.TransformPoint(point, worldObject);

                        bool detected = CarLines.FillContains(transformedPoint);

                        if (detected)
                        {
                            var detectedObject = new DetectedObjectInfo()
                            {
                                DetectedObject = worldObject,
                                Distance = 0
                            };
                            Packet.WorldObjectsDetected = new List<DetectedObjectInfo>() { detectedObject };
                            if (worldObject.WorldObjectType == WorldObjectType.Pedestrian)
                            {
                                CollidedWNPCEvent?.Invoke(detectedObject);
                            }
                            if (worldObject.WorldObjectType == WorldObjectType.Building)
                            {
                                CollidedWBuildingsEvent?.Invoke(detectedObject);
                            }
                            return;
                        }
                    }
                }
            }
        }
    }
}
