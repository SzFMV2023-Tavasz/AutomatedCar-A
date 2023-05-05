namespace AutomatedCar.SystemComponents
{
    using System;
    using System.Diagnostics;
    using System.Runtime.Intrinsics.Arm;
    using AutomatedCar.Models;
    using AutomatedCar.SystemComponents.Packets;
    using Avalonia.Input.TextInput;

    public class Characteristics : SystemComponent
    {
        private const double TireDiameter = 24; // 24-inch wheel size converted to circumference
        private const double NaturalDecelerationRate = 10;
        private const double KmphToMphRatio = 1.609344; // The conversion ratio between kilometer/h and mile/h

        private CharacteristicsPacket characteristicsPacket;
        private byte lastInnerGear;
        private double lastGearRatio;
        private IGearboxInterface gearboxPacket;
        private IPedalInterface gasPedalPacket;
        private IPedalInterface brakePedalPacket;

        private int previousRPM;


        public Characteristics(VirtualFunctionBus virtualFunctionBus)
            : base(virtualFunctionBus)
        {

            this.characteristicsPacket = new CharacteristicsPacket();
            this.characteristicsPacket.Speed = 0;
            this.characteristicsPacket.RPM = 540;
            virtualFunctionBus.CharacteristicsPacket = this.characteristicsPacket;
            
            this.gearboxPacket = virtualFunctionBus.GearboxPacket;
            this.gasPedalPacket = virtualFunctionBus.GasPedalPacket;
            this.brakePedalPacket = virtualFunctionBus.BrakePedalPacket;
            this.lastInnerGear = this.gearboxPacket.InnerGear;
            this.lastGearRatio = this.GetGearRatio();

            this.previousRPM = 0;
        }

        public override void Process()
        {
            double currentGearRatio = this.GetGearRatio();
            double currentSpeed = this.characteristicsPacket.Speed;
            byte gasPedalState = virtualFunctionBus.GasPedalPacket.PedalPosition;
            byte brakePedalState = virtualFunctionBus.BrakePedalPacket.PedalPosition;

            CalculateRPM(currentGearRatio);
            virtualFunctionBus.CharacteristicsPacket = this.characteristicsPacket;

        }

        public double GetGearRatio()
        {
            byte currentInnerGear = this.virtualFunctionBus.GearboxPacket.InnerGear;
            switch (currentInnerGear)
            {
                case 1:
                    return 1.0;
                case 2:
                    return 0.8;
                case 3:
                    return 0.6;
                case 4:
                    return 0.4;
                case 5:
                    return 0.2;
                default:
                    return 1.0;
            }
        }

        public int CalculateRPMDifference(int gasPedalState, int breakPedalState, double currentGearRatio)
        {
            if (this.gasPedalPacket.PedalPosition == 0 && this.characteristicsPacket.RPM >= 540)
            {

                if (this.brakePedalPacket.PedalPosition == 0)
                {
                    return -15;
                }

                else
                {
                    return -50;
                }
            }

            int dif = (int)Math.Sqrt((gasPedalState) * currentGearRatio);

            return dif;
        }

        public void CalculateRPM(double currentGearRatio)
        {
            if (this.characteristicsPacket.RPM >= 1500 && this.gearboxPacket.InnerGear < 5)
            {
                this.characteristicsPacket.RPM = 601;
            }
            else if (this.characteristicsPacket.RPM <= 600 && this.gearboxPacket.InnerGear > 1)
            {
                this.characteristicsPacket.RPM = 1499;
            }
            else
            {
                this.previousRPM = this.characteristicsPacket.RPM;
                this.characteristicsPacket.RPM += CalculateRPMDifference(this.gasPedalPacket.PedalPosition, this.brakePedalPacket.PedalPosition, currentGearRatio);
                World.Instance.ControlledCar.Revolution = this.characteristicsPacket.RPM;
                Debug.WriteLine("Gear: " + this.virtualFunctionBus.GearboxPacket.InnerGear);

                this.characteristicsPacket.Speed = ((this.gearboxPacket.InnerGear - 1) * 900 + this.characteristicsPacket.RPM - 540)/50;
                Debug.WriteLine("RPM: " + this.characteristicsPacket.RPM);
                Debug.WriteLine("Speed: " + this.characteristicsPacket.Speed);
            }
        }


        public double CalculateSpeed(double currentGearRatio)
        {
            double currentRPM = this.characteristicsPacket.RPM;
            double speed = ((TireDiameter * currentRPM) / (currentGearRatio * 366)) * KmphToMphRatio;
            this.characteristicsPacket.Speed = (float)speed;
            return speed;
        }
    }
}
