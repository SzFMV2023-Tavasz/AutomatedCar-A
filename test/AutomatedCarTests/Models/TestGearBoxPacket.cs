namespace Tests.Models
{
    using AutomatedCar.SystemComponents.Packets;

    // using AutomatedCarTests.SystemComponents;
    public class TestGearBoxPacket : GearBoxPacket
    {
        public void SetInnerGear(byte innerGear)
        {
            this.InnerGear = innerGear;
        }
    }
}
