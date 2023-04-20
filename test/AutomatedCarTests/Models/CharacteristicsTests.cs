
using System.Reactive;

namespace AutomatedCarTests.SystemComponents
{

    using AutomatedCar.SystemComponents;
    using AutomatedCar.SystemComponents.Packets;
    using NUnit.Framework;
    using AutomatedCar.SystemComponents.Packets;

    public class TestGearBoxPacket : GearBoxPacket
    {
        public void SetInnerGear(byte innerGear)
        {
            this.InnerGear = innerGear;
        }
    }

    [TestFixture]
    public class CharacteristicsTests
    {
        private VirtualFunctionBus virtualFunctionBus;

        [SetUp]
        public void Setup()
        {
            virtualFunctionBus = new VirtualFunctionBus();
            virtualFunctionBus.GearboxPacket = new TestGearBoxPacket();
            virtualFunctionBus.GasPedalPacket = new GasPedalPacket();
            virtualFunctionBus.BrakePedalPacket = new BrakePedalPacket();
        }

        //[Test]
        //public void Characteristics_InitialRpmAndSpeed_AreZero()
        //{
        //    var characteristics = new Characteristics(virtualFunctionBus);
        //    characteristics.Process();

        //    Assert.AreEqual(0, characteristics.characteristicsPacket.RPM);
        //    Assert.AreEqual(0, characteristics.characteristicsPacket.Speed);
        //}

        //[Test]
        //public void Characteristics_GasPedalPressed_SpeedAndRpmIncrease()
        //{
        //    virtualFunctionBus.GasPedalPacket.PedalPosition = 50;

        //    var characteristics = new Characteristics(virtualFunctionBus);
        //    characteristics.Process();

        //    Assert.IsTrue(characteristics.characteristicsPacket.RPM > 0);
        //    Assert.IsTrue(characteristics.characteristicsPacket.Speed > 0);
        //}

        //[Test]
        //public void Characteristics_BrakePedalPressed_SpeedAndRpmDecrease()
        //{
        //    virtualFunctionBus.GasPedalPacket.PedalPosition = 50;

        //    var characteristics = new Characteristics(virtualFunctionBus);
        //    characteristics.Process();

        //    var prevRPM = characteristics.characteristicsPacket.RPM;
        //    var prevSpeed = characteristics.characteristicsPacket.Speed;

        //    virtualFunctionBus.GasPedalPacket.PedalPosition = 0;
        //    virtualFunctionBus.BrakePedalPacket.PedalPosition = 50;
        //    characteristics.Process();

        //    Assert.IsTrue(characteristics.characteristicsPacket.RPM < prevRPM);
        //    Assert.IsTrue(characteristics.characteristicsPacket.Speed < prevSpeed);
        //}

        [Test]
        public void Characteristics_GearChanged_GearRatioChanges()
        {
            (virtualFunctionBus.GearboxPacket as TestGearBoxPacket).InnerGear = 3;

            var characteristics = new Characteristics(virtualFunctionBus);
            double gearRatio = characteristics.GetGearRatio();

            Assert.AreEqual(1.36, gearRatio);
        }

        [Test]
        public void Characteristics_InvalidGear_DefaultGearRatio()
        {
            (virtualFunctionBus.GearboxPacket as TestGearBoxPacket).InnerGear = 7;

            var characteristics = new Characteristics(virtualFunctionBus);
            double gearRatio = characteristics.GetGearRatio();

            Assert.AreEqual(1, gearRatio);
        }
    }
}
