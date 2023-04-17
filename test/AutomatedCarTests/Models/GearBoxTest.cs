using AutomatedCar.SystemComponents;
using Xunit;
namespace AutomatedCarTests.Models
{
    public class GearBoxTest
    {
        [Fact]
        public void ShiftUpTest()
        {
            GearBox gb = new GearBox(new VirtualFunctionBus());
            gb.Shift(1);
            byte actualGear = gb.gearBoxPacket.InnerGear;
            byte expectedGear = 2;
            Assert.Equal(expectedGear, actualGear);
        }
        [Fact]
        public void ShiftDownTest()
        {
            GearBox gb = new GearBox(new VirtualFunctionBus());
            gb.gearBoxPacket.InnerGear = 2;
            gb.Shift(-1);
            byte actualGear = gb.gearBoxPacket.InnerGear;
            byte expectedGear = 1;
            Assert.Equal(expectedGear, actualGear);
        }
    }
}
