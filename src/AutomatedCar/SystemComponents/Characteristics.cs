﻿namespace AutomatedCar.SystemComponents
{
    using System;
    using System.Diagnostics;
    using System.Runtime.Intrinsics.Arm;
    using System.Security;
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

        private double doubleRPM;


        public Characteristics(VirtualFunctionBus virtualFunctionBus)
            : base(virtualFunctionBus)
        {

            this.characteristicsPacket = new CharacteristicsPacket();
            this.characteristicsPacket.Speed = 0;
            this.characteristicsPacket.RPM = 600;
            virtualFunctionBus.CharacteristicsPacket = this.characteristicsPacket;
            
            this.gearboxPacket = virtualFunctionBus.GearboxPacket;
            this.gasPedalPacket = virtualFunctionBus.GasPedalPacket;
            this.brakePedalPacket = virtualFunctionBus.BrakePedalPacket;
            this.lastInnerGear = this.gearboxPacket.InnerGear;
            this.lastGearRatio = this.GetGearRatio();

            this.doubleRPM = 600;
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

        public double CalculateRPMDifference(int gasPedalState, int breakPedalState, double currentGearRatio)
        {
            //double dif = (double)Math.Sqrt(gasPedalState * currentGearRatio) * 1.85;
            double dif = (double)((double)gasPedalState * currentGearRatio) * 0.7;
            return dif;
        }

        public void CalculateRPM(double currentGearRatio)
        {
            // Gear change handling
            if (this.lastInnerGear < this.gearboxPacket.InnerGear)
            {
                this.characteristicsPacket.RPM = 601;
                doubleRPM = 601;
            }
            else if (this.gearboxPacket.InnerGear > this.gearboxPacket.InnerGear)
            {
                this.characteristicsPacket.RPM = 3999;
                doubleRPM = 3999;
            }
            //Acceleration
            else if (this.characteristicsPacket.RPM <= 7000)
            {
                this.doubleRPM += CalculateRPMDifference(this.gasPedalPacket.PedalPosition, this.brakePedalPacket.PedalPosition, currentGearRatio);
                this.characteristicsPacket.RPM = (int)doubleRPM;
                World.Instance.ControlledCar.Revolution = this.characteristicsPacket.RPM;
                this.characteristicsPacket.Speed = ((this.gearboxPacket.InnerGear - 1) * 3400 + this.characteristicsPacket.RPM - 600)/128;

                // Debug
                Debug.WriteLine("Gear: " + this.virtualFunctionBus.GearboxPacket.InnerGear);
                Debug.WriteLine("RPM: " + this.characteristicsPacket.RPM);
                Debug.WriteLine("Speed: " + this.characteristicsPacket.Speed);
            }
            this.lastInnerGear = this.gearboxPacket.InnerGear;
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
