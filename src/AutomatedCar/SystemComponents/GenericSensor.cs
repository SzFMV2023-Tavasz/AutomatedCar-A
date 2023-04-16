namespace AutomatedCar.SystemComponents
{
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
            double x0, y0, x1, y1, x2, y2;
             
            double outerPointDistance = ViewDistance / Math.Cos(this.DegToRad(this.FOV / 2));
            
            x0 = this.CarAnchorPoint.X + this.Car.X - this.Car.Geometry.Bounds.TopLeft.X;
            y0 = this.CarAnchorPoint.Y + this.Car.Y - this.Car.Geometry.Bounds.TopLeft.Y;
            x1 = (outerPointDistance * pixelToMeter * Math.Sin(this.DegToRad((-this.FOV / 2) + this.Car.Rotation + 180))) + this.Car.X;
            y1 = (outerPointDistance * pixelToMeter * Math.Cos(this.DegToRad((-this.FOV / 2) + this.Car.Rotation + 180))) + this.Car.Y;
            x2 = (outerPointDistance * pixelToMeter * Math.Sin(this.DegToRad((this.FOV / 2) + this.Car.Rotation + 180))) + this.Car.X;
            y2 = (outerPointDistance * pixelToMeter * Math.Cos(this.DegToRad((this.FOV / 2) + this.Car.Rotation + 180))) + this.Car.Y;



            return new List<Point>()
            {
                new Point(x0, y0),
                new Point(x1, y1),
                new Point(x2, y2),
            };
        }

        private IReadOnlyCollection<DetectedObjectInfo> DetectInSensorZone(List<Point> triangle)
        {
            List<DetectedObjectInfo> detectedObjects = new List<DetectedObjectInfo>();
            PolylineGeometry sensor = new PolylineGeometry(triangle, false);
            foreach (var worldObject in World.Instance.WorldObjects.Where(obj => this.WorldObjectTypesFilter.Contains(obj.WorldObjectType)))
            {
                if (worldObject.Geometries.Count > 0)
                {
                    foreach (var point in worldObject.Geometries[0].Points)
                    {
                        // Every boundary boxes at the origo, so needs to be transformed at its position
                        Point transformedPoint = new Point(point.X + worldObject.X, point.Y + worldObject.Y);
                        bool detected = sensor.FillContains(transformedPoint);

                        if (detected)
                        {
                            this.ProcessInformation(worldObject, transformedPoint, detectedObjects);
                        }
                    }
                }
            }

            return detectedObjects.AsReadOnly();
        }

        private void ProcessInformation(WorldObject worldObject, Point transformedPoint, List<DetectedObjectInfo> detectedObjects)
        {
            DetectedObjectInfo newInfo = new DetectedObjectInfo()
            {
                DetectedObject = worldObject,
                Distance = this.CalculateDistance(transformedPoint, this.CarAnchorPoint + new Point(this.Car.X, this.Car.Y)),
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
        // More debug needed
        private float CalculateDistance(Point transformedPointInWorld, Point carAnchorPointInWorld)
        {
            var vector = new Vector2((float)(transformedPointInWorld.X - carAnchorPointInWorld.X), (float)(transformedPointInWorld.Y - carAnchorPointInWorld.Y));

            var length = vector.Length();

            return length;
        }

        private double DegToRad(double degrees)
        {
            double radians = (Math.PI / 180) * degrees;

            return radians;
        }
    }
}
