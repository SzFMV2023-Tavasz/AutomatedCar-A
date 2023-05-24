namespace AutomatedCar.SystemComponents
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using AutomatedCar.Helpers;
    using AutomatedCar.Models;
    using AutomatedCar.SystemComponents.Packets;
    using Avalonia;
    using Avalonia.Media;

    public class LaneKeepingAssistance : SystemComponent
    {
        private LKAPacket packet;
        private CameraSensor cameraSensor;
        private AutomatedCar car;

        public bool isEnabled { get; set; }

        public LaneKeepingAssistance(AutomatedCar car, CameraSensor cameraSensor, VirtualFunctionBus virtualFunctionBus)
            : base(virtualFunctionBus)
        {
            this.cameraSensor = cameraSensor;
            this.car = car;

            this.packet = new LKAPacket();
            this.packet.recommendedTurnAngle = 0;
            this.virtualFunctionBus.LaneKeepingPacket = this.packet;
        }

        public bool canBeEnabled()
        {
            // egyelore csak
            // hogy buildeljen
            return true;
        }

        public override void Process()
        {
            this.packet.recommendedTurnAngle = this.GetRecommendedTurnAngle();
        }

        public Point GetLaneCenterPoint()
        {
            List<DetectedObjectInfo> detectedObjects = new List<DetectedObjectInfo>();
            PolylineGeometry sensor = new PolylineGeometry(this.cameraSensor.GenerateSensorTriangle(), false);
            var filteredWorldObjects = World.Instance.WorldObjects.Where(obj => obj.WorldObjectType == WorldObjectType.Road && !obj.Equals(this.car));

            this.cameraSensor.DetectObjects(detectedObjects, sensor, filteredWorldObjects);

            detectedObjects = detectedObjects?.OrderBy(x => x.Distance).Take(2).ToList();
            double x1, y1, x2, y2;
            x1 = detectedObjects[0].DetectedObject.X;
            y1 = detectedObjects[0].DetectedObject.Y;
            x2 = detectedObjects[1].DetectedObject.X;
            y2 = detectedObjects[1].DetectedObject.Y;
            return new Point((x1 + x2) / 2, (y1 + y2) / 2);
        }

        // More debug needed
        public double GetRecommendedTurnAngle()
        {
            Point targetPoint = this.GetLaneCenterPoint();

            double targetRotation = GeometryUtils.GetRotation(
                GeometryUtils.RadToDeg(
                    GeometryUtils.GetAngle(targetPoint, new Point(this.car.X, this.car.Y))));

            double recommendedTurnAngle = targetRotation - this.car.Rotation;
            return (Math.Abs(recommendedTurnAngle) <= 45) ? recommendedTurnAngle : double.NaN;
        }
    }
}
