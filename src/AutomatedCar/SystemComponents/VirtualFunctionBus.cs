namespace AutomatedCar.SystemComponents
{
    using AutomatedCar.Models;
    using AutomatedCar.SystemComponents.Packets;
    using System.Collections.Generic;

    public class VirtualFunctionBus : GameBase
    {
        private List<SystemComponent> components = new List<SystemComponent>();

        public IReadOnlyDummyPacket DummyPacket { get; set; }

        public ICharacteristicsInterface CharacteristicsPacket { get; set; }
        public IPedalInterface BrakePedalPacket { get; set; }
        public IPedalInterface GasPedalPacket { get; set; }

        public IGearboxInterface GearboxPacket { get; set; }

        public ISteeringWheel SteeringWheelPacket { get; set; }

        public IDrivechain drivechainPacket { get; set; }
        public IAccInterface AccPacket { get; set; }

        public IAEBInterface AEBPacket { get; set; }

        public IReadOnlyPacket<DetectedObjectInfo> RadarPacket { get; set; }
        public IReadOnlyPacket<DetectedObjectInfo> CameraPacket { get; set; }
        public IReadOnlyPacket<DetectedObjectInfo> CollisionPacket { get; set; }

        public IReadOnlyPacket<DetectedObjectInfo> OnWayToCollidePacket { get; set; }

        public ILaneKeepingPacket LaneKeepingPacket { get; set; }

        public ILKANotifierPacket LKANotifierPacket { get; set; }
        public ILKAInfoPacket LKAStatusPacket { get; set; }

        public void RegisterComponent(SystemComponent component)
        {
            this.components.Add(component);
        }

        protected override void Tick()
        {
            foreach (SystemComponent component in this.components)
            {
                component.Process();
            }
        }
    }
}