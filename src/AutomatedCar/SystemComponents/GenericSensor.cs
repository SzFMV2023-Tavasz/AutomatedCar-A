namespace AutomatedCar.SystemComponents
{
    using AutomatedCar.Helpers;
    using AutomatedCar.Models;
    using AutomatedCar.SystemComponents.Packets;
    using Avalonia;
    using Avalonia.Media;
    using JetBrains.Annotations;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Net.Sockets;
    using System.Numerics;

    public abstract class GenericSensor : SystemComponent
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="GenericSensor"/> class.
        /// </summary>
        /// <param name="sensorSettings">This contains all the information thats needed for sensor constructing.</param>

        public GenericSensor(SensorSettings sensorSettings)
                        : base(sensorSettings.FunctionBus)
        {
            this.Car = sensorSettings.Car;
            this.CarAnchorPoint = sensorSettings.CarAnchorPoint;
            this.FOV = sensorSettings.FieldOfView;
            this.ViewDistance = sensorSettings.ViewDistance;
            this.WorldObjectTypesFilter = sensorSettings.WorldObjectFilter;
            this.Packet = new ReadOnlyPacket();
        }

        /// <summary>
        /// Gets or sets Where the sensor is placed.
        /// </summary>
        public Point CarAnchorPoint { get; set; }

        /// <summary>
        /// Gets or sets The sensor's Field of View.
        /// </summary>
        public double FOV { get; set; }

        /// <summary>
        /// Gets or sets The Sensor's ViewDistance.
        /// </summary>
        public double ViewDistance { get; set; }

        /// <summary>
        /// Gets or sets A filter for the simulation. This tells the sensor what kind of objects this object can detect.
        /// </summary>
        public IEnumerable<WorldObjectType> WorldObjectTypesFilter { get; set; }

        /// <summary>
        /// Gets or sets This is where it write the detected objects
        /// </summary>
        protected IReadOnlyPacket<DetectedObjectInfo> Packet { get; set; }

        /// <summary>
        /// Gets the reference which car owns this sensor.
        /// </summary>
        protected AutomatedCar Car { get; }


        public override void Process()
        {
            List<Point> triangle = this.GenerateSensorTriangle();

            var detectedObjList = this.DetectInSensorZone(triangle);

            this.Packet.WorldObjectsDetected = detectedObjList;
        }

        public List<Point> GenerateSensorTriangle()
        {
            const int pixelToMeter = 50;
            double x1, y1, x2, y2;

            double outerPointDistance = ViewDistance / Math.Cos(GeometryUtils.DegToRad(this.FOV / 2));

            Point transformedAnchorPoint = GeometryUtils.TransformPoint(this.CarAnchorPoint, this.Car);
            x1 = (outerPointDistance * pixelToMeter * Math.Sin(GeometryUtils.DegToRad((-this.FOV / 2) - this.Car.Rotation + 180))) + this.Car.X;
            y1 = (outerPointDistance * pixelToMeter * Math.Cos(GeometryUtils.DegToRad((-this.FOV / 2) - this.Car.Rotation + 180))) + this.Car.Y;
            x2 = (outerPointDistance * pixelToMeter * Math.Sin(GeometryUtils.DegToRad((this.FOV / 2) - this.Car.Rotation + 180))) + this.Car.X;
            y2 = (outerPointDistance * pixelToMeter * Math.Cos(GeometryUtils.DegToRad((this.FOV / 2) - this.Car.Rotation + 180))) + this.Car.Y;

            return new List<Point>()
            {
                transformedAnchorPoint,
                new Point(x1, y1),
                new Point(x2, y2),
            };
        }

        private IReadOnlyCollection<DetectedObjectInfo> DetectInSensorZone(List<Point> triangle)
        {
            List<DetectedObjectInfo> detectedObjects = new List<DetectedObjectInfo>();
            PolylineGeometry sensor = new PolylineGeometry(triangle, false);
            var filteredWorldObjects = World.Instance.WorldObjects.Where(obj => this.WorldObjectTypesFilter.Contains(obj.WorldObjectType) && !obj.Equals(this.Car));
            foreach (var worldObject in filteredWorldObjects)
            {
                foreach (var geometry in worldObject.Geometries)
                {
                    foreach (var point in geometry.Points)
                    {
                        // Every boundary boxes at the origo, so needs to be transformed at its position
                        Point transformedPoint = GeometryUtils.TransformPoint(point, worldObject);
                        bool detected = sensor.FillContains(transformedPoint);

                        if (detected)
                        {
                            this.ProcessInformation(worldObject, transformedPoint, detectedObjects);
                        }
                    }
                }
            }

            detectedObjects = detectedObjects?.OrderBy(x => x.Distance).ToList();
            return detectedObjects.AsReadOnly();
        }

        private void ProcessInformation(WorldObject worldObject, Point transformedPoint, List<DetectedObjectInfo> detectedObjects)
        {
            DetectedObjectInfo newInfo = new DetectedObjectInfo()
            {
                DetectedObject = worldObject,
                Distance = (float)GeometryUtils.GetEuclidianDistance(transformedPoint, this.CarAnchorPoint + new Point(this.Car.X, this.Car.Y)),
            };
            if (!detectedObjects.Contains(newInfo))
            {
                detectedObjects.Add(newInfo);
            }
            else
            {
                var info = detectedObjects.Find(x => x.Equals(newInfo));
                if (info.Distance > newInfo.Distance)
                {
                    detectedObjects.Remove(info);
                    detectedObjects.Add(newInfo);
                }
            }
        }

        public Point GetLaneCenterPoint(List<Point> triangle)
        {
            List <DetectedObjectInfo> detectedObjects = new List<DetectedObjectInfo>();
            PolylineGeometry sensor = new PolylineGeometry(triangle, false);
            var filteredWorldObjects = World.Instance.WorldObjects.Where(obj => obj.WorldObjectType == WorldObjectType.Road && !obj.Equals(this.Car));
            foreach (var worldObject in filteredWorldObjects)
            {
                foreach (var geometry in worldObject.Geometries)
                {
                    foreach (var point in geometry.Points)
                    {
                        // Every boundary boxes at the origo, so needs to be transformed at its position
                        Point transformedPoint = GeometryUtils.TransformPoint(point, worldObject);
                        bool detected = sensor.FillContains(transformedPoint);

                        if (detected)
                        {
                            this.ProcessInformation(worldObject, transformedPoint, detectedObjects);
                        }
                    }
                }
            }

            detectedObjects = detectedObjects?.OrderBy(x => x.Distance).Take(2).ToList();
            double x1, y1, x2, y2;
            x1 = detectedObjects[0].DetectedObject.X;
            y1 = detectedObjects[0].DetectedObject.Y;
            x2 = detectedObjects[1].DetectedObject.X;
            y2 = detectedObjects[1].DetectedObject.Y;
            return new Point((x1 + x2) / 2, (y1 + y2) / 2);
        }

        public double GetRecommendedTurnAngle()
        {
            var triangle = this.GenerateSensorTriangle();
            Point targetPoint = this.GetLaneCenterPoint(triangle);

            double targetRotation = GeometryUtils.GetRotation(
                GeometryUtils.RadToDeg(
                    GeometryUtils.GetAngle(targetPoint, new Point(this.Car.X, this.Car.Y))));

            return targetRotation - this.Car.Rotation;
        }

        // More debug needed
    }
}
