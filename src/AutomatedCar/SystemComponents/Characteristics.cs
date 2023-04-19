namespace AutomatedCar.SystemComponents
{
    using System;
    using AutomatedCar.SystemComponents.Packets;

    public class Characteristics : SystemComponent
    {
        private const double TireDiameter = 24; // 24-inch wheel size converted to circumference
        private const double NaturalDecelerationRate = 10;
        private const double KmphToMphRatio = 1.609344; // The conversion ratio between kilometer/h and mile/h

        public CharacteristicsPacket characteristicsPacket;
        private byte lastInnerGear;
        private double lastGearRatio;

        public Characteristics(VirtualFunctionBus virtualFunctionBus)
            : base(virtualFunctionBus)
        {
            this.characteristicsPacket = new CharacteristicsPacket();
            virtualFunctionBus.CharacteristicsPacket = this.characteristicsPacket;
            this.lastInnerGear = this.virtualFunctionBus.GearboxPacket.InnerGear;
            this.lastGearRatio = this.GetGearRatio();
            this.characteristicsPacket.Speed = 0;
            this.characteristicsPacket.RPM = 0;
        }

        public override void Process()
        {
            double currentGearRatio = this.GetGearRatio();
            double currentSpeed = this.characteristicsPacket.Speed;
            byte gasPedalState = virtualFunctionBus.GasPedalPacket.PedalPosition;
            byte brakePedalState = virtualFunctionBus.BrakePedalPacket.PedalPosition;

            this.characteristicsPacket.RPM = CalculateRPM(currentGearRatio) +
                CalculateRPMDifference(gasPedalState, brakePedalState, currentGearRatio);
            this.characteristicsPacket.Speed = (float)CalculateSpeed(currentGearRatio);
        }

        public double GetGearRatio()
        {
            byte currentInnerGear = this.virtualFunctionBus.GearboxPacket.InnerGear;
            switch (currentInnerGear)
            {
                case 1:
                    return 3.79;
                case 2:
                    return 2.07;
                case 3:
                    return 1.36;
                case 4:
                    return 1.03;
                case 5:
                    return 1;
                default:
                    return 1;
            }
        }

        public int CalculateRPMDifference(int gasPedalState, int breakPedalState, double currentGearRatio)
        {
            return (int)Math.Sqrt((gasPedalState - breakPedalState - NaturalDecelerationRate) * currentGearRatio) * 5;
        }

        public int CalculateRPM(double currentGearRatio)
        {
            double currentSpeed = this.characteristicsPacket.Speed / KmphToMphRatio; // Speed need to be in mph in the calculation
            return (int)(Math.Round(currentSpeed * currentGearRatio * 336) / TireDiameter);
        }

        public double CalculateSpeed(double currentGearRatio)
        {
            double currentRPM = this.characteristicsPacket.RPM;
            return ((TireDiameter * currentRPM) / (currentGearRatio * 366)) * KmphToMphRatio;
        }
    }
}
