namespace AutomatedCar.SystemComponents
{
    using AutomatedCar.Models;
    using AutomatedCar.SystemComponents.Packets;
    using Avalonia;
    using Avalonia.Media;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;

    public abstract class GenericSensor : SystemComponent
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="GenericSensor"/> class.
        /// </summary>
        /// <param name="car">Reference for the car who own this</param>
        /// <param name="virtualFunctionBus">Reference for the VirtualFunctionBus</param>
        /// <param name="carAnchorPoint">The point where its been placed on the car.</param>
        /// <param name="fOV">The sensor's field of view</param>
        /// <param name="viewDistance">The sensor's view distance</param>
        /// <param name="worldObjectTypesFilter">Filter for detecting</param>
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
            return new List<Point>()
            {
                new Point(0 + this.Car.X,0 + this.Car.Y),
                new Point((92.37f * 50 * Math.Sin(this.DegToRad((-this.FOV / 2) + this.Car.Rotation + 180))) + this.Car.X, (92.37f * 50 * Math.Cos(this.DegToRad((-this.FOV / 2) + this.Car.Rotation + 180))) + this.Car.Y),
                new Point((92.37f * 50 * Math.Sin(this.DegToRad((this.FOV / 2) + this.Car.Rotation + 180))) + this.Car.X, (92.37f * 50 * Math.Cos(this.DegToRad((this.FOV / 2) + this.Car.Rotation + 180))) + this.Car.Y),
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
                        Point transformedPoint = new Point(point.X + worldObject.X, point.Y + worldObject.Y);
                        bool detected = sensor.FillContains(transformedPoint);

                        if (detected)
                        {
                            DetectedObjectInfo newInfo = new DetectedObjectInfo()
                            {
                                DetectedObject = worldObject,
                                Distance = 0,
                            };
                            if (!detectedObjects.Contains(newInfo))
                            {
                                detectedObjects.Add(newInfo);
                            }
                        }
                    }
                }
            }

            return detectedObjects.AsReadOnly();
        }

        private double DegToRad(double degrees)
        {
            double radians = (Math.PI / 180) * degrees;

            return radians;
        }
    }
}
