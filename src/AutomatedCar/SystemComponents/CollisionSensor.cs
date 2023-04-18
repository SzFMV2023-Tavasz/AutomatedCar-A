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
                CarPoints.Add(new Point(point.X+car.X-car.Geometry.Bounds.Center.X,point.Y+car.Y - car.Geometry.Bounds.Center.Y));
            }
            PolylineGeometry CarLines = new PolylineGeometry(CarPoints, false);

            var collidibleWorldObjects = World.Instance.WorldObjects.Where(obj => this.WorldObjectTypesFilter.Contains(obj.WorldObjectType) && !obj.Equals(this.car));
            foreach (var worldObject in collidibleWorldObjects)
            {
                if (worldObject.Geometries.Count > 0)
                {
                    foreach (var point in worldObject.Geometries[0].Points)
                    {
                        // Every boundary boxes at the origo, so needs to be transformed at its position
                        Point transformedPoint = new Point(point.X + worldObject.X, point.Y + worldObject.Y);
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
    }
}
