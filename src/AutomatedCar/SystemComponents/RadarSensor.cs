namespace AutomatedCar.SystemComponents
{
    using AutomatedCar.Models;
    using AutomatedCar.SystemComponents.Packets;
    using Avalonia;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class RadarSensor : GenericSensor
    {
        private IReadOnlyPacket<DetectedObjectInfo> onWayToCollidePacket;

        private IReadOnlyPacket<DetectedObjectInfo> previousTickPacket;

        public RadarSensor(SensorSettings sensorSettings)
            : base(sensorSettings)
        {
            sensorSettings.FunctionBus.RadarPacket = this.Packet;
            this.onWayToCollidePacket = new ReadOnlyPacket();
            this.previousTickPacket = new ReadOnlyPacket();
            onWayToCollidePacket.WorldObjectsDetected = new List<DetectedObjectInfo>();

            previousTickPacket.WorldObjectsDetected = new List<DetectedObjectInfo>();
            Packet.WorldObjectsDetected = new List<DetectedObjectInfo>();
            sensorSettings.FunctionBus.OnWayToCollidePacket = this.onWayToCollidePacket;
        }

        public override void Process()
        {
            CopyRecentToPreviousPacket();
            base.Process();

            this.DetectFutureCollision();
        }

        private void CopyRecentToPreviousPacket()
        {
            this.previousTickPacket.WorldObjectsDetected = this.Packet.WorldObjectsDetected.Select(info => new DetectedObjectInfo()
            {
                DetectedObject = info.DetectedObject,
                Distance = info.Distance,
            }).ToList().AsReadOnly();
        }

        private void DetectFutureCollision()
        {
            List<DetectedObjectInfo> newFutureCollisionInfo = new List<DetectedObjectInfo>();

            foreach (DetectedObjectInfo info in this.Packet.WorldObjectsDetected)
            {
                if (this.IsReleventObjectInfo(info))
                {
                    double ditanceInPreviousDetection = this.previousTickPacket.WorldObjectsDetected.FirstOrDefault(x => x.DetectedObject == info.DetectedObject).Distance;

                    double distanceDifferenceBetweenTicks = info.Distance - ditanceInPreviousDetection;

                    
                    if (IsClosingOnUs(distanceDifferenceBetweenTicks))
                    {
                        newFutureCollisionInfo.Add(info);
                    }
                }
            }

            this.onWayToCollidePacket.WorldObjectsDetected = newFutureCollisionInfo.AsReadOnly();

            Debug.WriteLineIf(newFutureCollisionInfo.Count > 0, "---------------");
            foreach (var item in onWayToCollidePacket.WorldObjectsDetected)
            {
                Debug.WriteLine(item.DetectedObject.Filename + " " + item.Distance);
            }

        }

        private bool IsClosingOnUs(double distanceDifferenceBetweenTicks)
        {
            return distanceDifferenceBetweenTicks < 0;
        }

        private bool IsReleventObjectInfo(DetectedObjectInfo info)
        {
            const int pixelToMeter = 50;
            const int distanceInLane = 10;
            return previousTickPacket.WorldObjectsDetected.Contains(info) && info.Distance < distanceInLane * pixelToMeter;
        }

    }
}
