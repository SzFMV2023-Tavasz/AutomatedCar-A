namespace AutomatedCar.SystemComponents
{
    using AutomatedCar.SystemComponents.Packets;
    using System.Collections.Generic;

    public class VirtualFunctionBus : GameBase
    {
        private List<SystemComponent> components = new List<SystemComponent>();

        public IReadOnlyDummyPacket DummyPacket { get; set; }
        public ICharacteristicsInterface CharacteristicsPacket { get; set; }

        public IGearboxInterface GearboxPacket { get; set; }

        public IGearboxInterface GearboxPacket { get; set; }

        public ISteeringWheel SteeringWheelPacket {get; set;}

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