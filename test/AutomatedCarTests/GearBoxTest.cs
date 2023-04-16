namespace Tests
{
    using AutomatedCar.SystemComponents;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    [TestClass]
    public class GearBoxTest
    {
        [TestMethod]
        public void ShiftUpTest()
        {
            GearBox gb = new GearBox(new VirtualFunctionBus());
            gb.Shift(1);
            byte actualGear = gb.gearBoxPacket.InnerGear;
            byte expectedGear = 2;
            Assert.AreEqual(expectedGear,actualGear);
        }
    }
}
