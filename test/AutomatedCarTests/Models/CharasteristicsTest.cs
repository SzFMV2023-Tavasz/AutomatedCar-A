namespace Tests.Models
{
    using AutomatedCar.SystemComponents;
    // using AutomatedCarTests.SystemComponents;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    [TestClass]
    class CharasteristicsTest
    {
        [TestMethod]
        [DataRow(1, 3.79)]
        [DataRow(2, 2.07)]
        [DataRow(3, 1.36)]
        [DataRow(4, 1.03)]
        [DataRow(5, 1)]
        [DataRow(10, 1)]
        public void TestGetGearRatio(byte gear, double result)
        {
            VirtualFunctionBus vfb = new VirtualFunctionBus();
            (vfb.GearboxPacket as TestGearBoxPacket).InnerGear = gear;
            Characteristics c = new Characteristics(vfb);
            var currentGearRatio = c.GetGearRatio();
            Assert.AreEqual(currentGearRatio, result);
        }




    }
}
