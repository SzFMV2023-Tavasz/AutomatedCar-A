namespace AutomatedCar.SystemComponents
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
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
        private static readonly string[] INVALID_ROADTYPES = {
            "road_2lane_rotary.png",
            "road_2lane_tjunctionleft.png",
            "road_2lane_tjunctionright.png",
            "road_2lane_crossroad_1.png",
            "road_2lane_crossroad_2.png",
        };

        private LKAPacket packet;
        private LKAInfoPacket lKAInfoPacket;
        private CameraSensor cameraSensor;
        private AutomatedCar car;
        private LKANotifierPacket notifierPacket;

        public bool IsEnabled { get; set; }

       private void SetLKAInfo()
        {
            if (!this.IsEnabled)
            {
                if (this.CanBeEnabled())
                {
                    lKAInfoPacket.Status  ="Available";
                }
                else
                {
                    lKAInfoPacket.Status = "Not Available";
                }
            }
            else
            {
                if (this.WillBeTurnOff())
                {
                    lKAInfoPacket.Status = "Will turn off";
                }
                else
                {
                    lKAInfoPacket.Status = "On";
                }
            }
        }

         

        public LaneKeepingAssistance(AutomatedCar car, CameraSensor cameraSensor, VirtualFunctionBus virtualFunctionBus)
            : base(virtualFunctionBus)
        {
            this.cameraSensor = cameraSensor;
            this.car = car;

            this.packet = new LKAPacket();
            this.notifierPacket = new LKANotifierPacket();

            virtualFunctionBus.LKANotifierPacket = this.notifierPacket;
            this.packet.recommendedTurnAngle = double.NaN;
            this.virtualFunctionBus.LaneKeepingPacket = this.packet;
            this.lKAInfoPacket = new LKAInfoPacket();
            this.virtualFunctionBus.LKAStatusPacket = this.lKAInfoPacket;
        }

        public override void Process()
        {
            this.TurnOnMechanism();
            this.TurnOffMechanism();
            this.SetLKAInfo();
            
            if (!this.IsEnabled)
            {
                this.NullPacketIfNecessary();
                return;
            }
            this.WillBeTurnOff();
            this.packet.recommendedTurnAngle = this.GetRecommendedTurnAngle();
            if (this.packet.recommendedTurnAngle == double.NaN)
            {
                this.IsEnabled = false;
            }
        }

        private bool WillBeTurnOff()
        {
            const int meter = 20;

            const int pixelToMeter = 50;

            const int distance = meter * pixelToMeter;

            var problematicObjectsInDistance = this.virtualFunctionBus.CameraPacket.WorldObjectsDetected.Where(x => INVALID_ROADTYPES.Contains(x.DetectedObject.Filename)).Any(y => y.Distance < distance);

            return problematicObjectsInDistance;
        }

        private void NullPacketIfNecessary()
        {
            if (this.packet.recommendedTurnAngle != double.NaN)
            {
                this.packet.recommendedTurnAngle = double.NaN;
            }
        }

        private void TurnOnMechanism()
        {
            if (this.CanBeEnabled() && Keyboard.IsKeyDown(Avalonia.Input.Key.L))
            {
                this.IsEnabled = !this.IsEnabled;
            }
            if (this.notifierPacket.Intervention)
            {
                this.IsEnabled = false;
                this.notifierPacket.Intervention = false;
            }
        }

        private void TurnOffMechanism()
        {
            const int meter = 10;

            const int pixelToMeter = 50;

            const int distance = meter * pixelToMeter;

            var problematicObjectsInDistance = this.virtualFunctionBus.CameraPacket.WorldObjectsDetected.Where(x => INVALID_ROADTYPES.Contains(x.DetectedObject.Filename)).Any(y => y.Distance < distance);

            if (problematicObjectsInDistance)
            {
                IsEnabled = false;
            }

        }

        private bool CanBeEnabled()
        {
            return this.GetRecommendedTurnAngle() != double.NaN;
        }


        private Point GetLaneCenterPoint(List<DetectedObjectInfo> lanes)
        {
            lanes = lanes?.OrderBy(x => x.Distance).Take(2).ToList();
            double x1, y1, x2, y2;
            x1 = lanes[0].DetectedObject.X;
            y1 = lanes[0].DetectedObject.Y;
            x2 = lanes[1].DetectedObject.X;
            y2 = lanes[1].DetectedObject.Y;

            return new Point((x1 + x2) / 2, (y1 + y2) / 2);
        }

        private List<DetectedObjectInfo> GetLanes()
        {
            List<DetectedObjectInfo> detectedObjects = new List<DetectedObjectInfo>();
            PolylineGeometry sensor = new PolylineGeometry(this.cameraSensor.GenerateSensorTriangle(), false);
            var filteredWorldObjects = World.Instance.WorldObjects.Where(obj => obj.WorldObjectType == WorldObjectType.Road
            && !INVALID_ROADTYPES.Contains(obj.Filename)
            && !obj.Equals(this.car));

            this.cameraSensor.DetectObjects(detectedObjects, sensor, filteredWorldObjects);

            return detectedObjects;
        }

        private double GetRecommendedTurnAngle()
        {
            var lanes = this.GetLanes();
            if (lanes.Count < 2)
            {
                return double.NaN;
            }

            Point targetPoint = this.GetLaneCenterPoint(lanes);

            double targetRotation = GeometryUtils.GetRotation(
                GeometryUtils.RadToDeg(
                    GeometryUtils.GetAngle(targetPoint, new Point(this.car.X, this.car.Y))));

            double recommendedTurnAngle = targetRotation - this.car.Rotation;
            return (Math.Abs(recommendedTurnAngle) <= 45) ? recommendedTurnAngle : double.NaN;
        }
    }
}