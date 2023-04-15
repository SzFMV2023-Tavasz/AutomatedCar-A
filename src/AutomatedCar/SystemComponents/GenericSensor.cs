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

    public class GenericSensor : SystemComponent
    {

        /// <summary>
        /// This is the default constructor for a generic purpose sensor.
        /// </summary>
        /// <param name="car">Reference for the car who own this</param>
        /// <param name="virtualFunctionBus">Reference for the VirtualFunctionBus</param>
        /// <param name="carAnchorPoint">The point where its been placed on the car.</param>
        /// <param name="fOV">The sensor's field of view</param>
        /// <param name="viewDistance">The sensor's view distance</param>
        /// <param name="worldObjectTypesFilter">Filter for detecting</param>
        public GenericSensor(AutomatedCar car, VirtualFunctionBus virtualFunctionBus,
                    Point carAnchorPoint,
                    double fOV, double viewDistance,
                    IEnumerable<WorldObjectType> worldObjectTypesFilter)
                        : base(virtualFunctionBus)
        {
            Car = car;
            CarAnchorPoint = carAnchorPoint;
            FOV = fOV;
            ViewDistance = viewDistance;
            WorldObjectTypesFilter = worldObjectTypesFilter;
            Packet = new ReadOnlyPacket();

        }
        /// <summary>
        /// Where the sensor is placed.
        /// </summary>
        public Point CarAnchorPoint { get; set; }
        /// <summary>
        /// The sensor's Field of View.
        /// </summary>
        public double FOV { get; set; }
        /// <summary>
        /// The Sensor's ViewDistance.
        /// </summary>
        public double ViewDistance { get; set; }
        /// <summary>
        /// A filter for the simulation. This tells the sensor what kind of objects this object can detect.
        /// </summary>
        public IEnumerable<WorldObjectType> WorldObjectTypesFilter { get; set; }
        /// <summary>
        /// This is where it write the detected objects
        /// </summary>
        protected IReadOnlyPacket<DetectedObjectInfo> Packet { get; set; }
        /// <summary>
        /// For reference which car owns this sensor.
        /// </summary>
        protected AutomatedCar Car { get; }


        public override void Process()
        {
            List<Point> triangle = this.GenerateSensorTriangle();

            // For debug purposes
            if (World.Instance.ControlledCar == this.Car)
            {
                for (int i = 0; i < 3; i++)
                {
                    Debug.WriteLine(string.Format("{0} => {1}, {2}", i, triangle[i].X, triangle[i].Y));
                }
            }

            var detectedObjList = this.DetectInSensorZone(triangle);

            // For debug purposes
            if (World.Instance.ControlledCar == this.Car)
            {
                this.DebugToConsole(detectedObjList);
            }

            this.Packet.WorldObjectsDetected = detectedObjList;
        }

        public List<Point> GenerateSensorTriangle()
        {
            return new List<Point>()
            {
                new Point(0 + Car.X,0 + Car.Y),
                new Point(92.37f *50* Math.Sin(DegToRad(-this.FOV/2 + this.Car.Rotation+ 180)) + Car.X, 92.37f *50* Math.Cos(DegToRad(-this.FOV/2 + this.Car.Rotation+180)) + Car.Y),
                new Point(92.37f * 50 * Math.Sin(DegToRad(this.FOV/2 + this.Car.Rotation+ 180)) + Car.X, 92.37f *50* Math.Cos(DegToRad(this.FOV/2 + this.Car.Rotation+180)) + Car.Y),
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
        private void DebugToConsole(IReadOnlyCollection<DetectedObjectInfo> detected)
        {
            Debug.WriteLine(string.Format("==={0}===", DateTime.Now.Ticks));
            foreach (var obj in detected)
            {
                Debug.WriteLine(string.Format("{0} -> ({1}, {2})", obj.DetectedObject.Filename, obj.DetectedObject.X, obj.DetectedObject.Y));
            }
        }
    }
}
