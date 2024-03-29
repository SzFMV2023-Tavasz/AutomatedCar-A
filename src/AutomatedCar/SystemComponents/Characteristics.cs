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

            //Debug.WriteLine("Gear: " + this.virtualFunctionBus.GearboxPacket.InnerGear);
            //Debug.WriteLine("RPM: " + this.characteristicsPacket.RPM);
            //Debug.WriteLine("Speed: " + this.characteristicsPacket.Speed);
            //Debug.WriteLine("fromSpeed: " + this.characteristicsPacket.RPM);
            //Debug.WriteLine("------------------" + this.gearboxPacket.ActualGear + "------------------");
        }

        public void Process_with_gaspedal_to_5()
        {
            double currentGearRatio = this.GetGearRatio();
            double currentSpeed = this.characteristicsPacket.Speed;
            byte gasPedalState = virtualFunctionBus.GasPedalPacket.PedalPosition;
            byte brakePedalState = virtualFunctionBus.BrakePedalPacket.PedalPosition;
            if (this.gasPedalPacket.PedalPosition < 5)
            {
                this.gasPedalPacket.PedalPosition = 5;
            }
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

        public double CalculateRPMDifference_with_auto_decrease(int gasPedalState, int breakPedalState, double currentGearRatio)
        {
            //double dif = (double)Math.Sqrt(gasPedalState * currentGearRatio) * 1.85;
            double dif;
            if (this.characteristicsPacket.RPM >= 606)
            {
                dif = (double)((double)gasPedalState * currentGearRatio) * 0.7 - 5;
            }
            else dif = (double)((double)gasPedalState * currentGearRatio) * 0.7;
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

            // DRIVE
            if (this.gearboxPacket.ActualGear == OuterGear.D || this.gearboxPacket.ActualGear == OuterGear.N)
            {
                //Acceleration
                if (this.characteristicsPacket.RPM <= 7000 && this.brakePedalPacket.PedalPosition == 0 && this.gasPedalPacket.PedalPosition > 0)
                {
                    if ((((this.gearboxPacket.InnerGear - 1) * 3400 + this.characteristicsPacket.RPM - 600) / 128) < 200) // < x -- AEB SPEEDLIMIT TEST
                    {
                        this.doubleRPM += CalculateRPMDifference(this.gasPedalPacket.PedalPosition, this.brakePedalPacket.PedalPosition, currentGearRatio);
                        this.characteristicsPacket.RPM = (int)doubleRPM;
                        World.Instance.ControlledCar.Revolution = this.characteristicsPacket.RPM;

                        if ((((this.gearboxPacket.InnerGear - 1) * 3400 + this.characteristicsPacket.RPM - 600) / 128) < 0.1)
                        {
                            this.characteristicsPacket.Speed = 0;
                        }

                        else if (this.gearboxPacket.ActualGear == OuterGear.D)
                        {
                            this.characteristicsPacket.Speed = ((this.gearboxPacket.InnerGear - 1) * 3400 + this.characteristicsPacket.RPM - 600) / 128;
                        }

                    }

                    else if(this.gearboxPacket.ActualGear == OuterGear.D)
                    {
                        this.characteristicsPacket.Speed = 200;
                    }

                    //Debug.WriteLine("Gear: " + this.virtualFunctionBus.GearboxPacket.InnerGear);
                    //Debug.WriteLine("RPM: " + this.characteristicsPacket.RPM);
                    //Debug.WriteLine("Speed: " + this.characteristicsPacket.Speed);
                    //Debug.WriteLine("fromSpeed: " + this.characteristicsPacket.RPM);
                    //Debug.WriteLine("------------------" + this.gearboxPacket.ActualGear + "------------------");
                }


                // Debug

                else if (this.brakePedalPacket.PedalPosition == 0 && this.gasPedalPacket.PedalPosition == 0 && this.characteristicsPacket.Speed == 0)
                {
                    Random r = new();
                    //int n = 0;
                    int n = r.Next(55, 74);
                    if (this.characteristicsPacket.RPM - n < 74 && this.characteristicsPacket.RPM != 601)
                    {
                        this.characteristicsPacket.RPM = 601;
                        this.doubleRPM = this.characteristicsPacket.RPM;
                    }

                    else if (this.characteristicsPacket.RPM != 601)
                    {
                        //Debug.WriteLine("RPM -------------------------------------------------------------------------------------------------------------------");
                        this.doubleRPM -= n;
                        this.characteristicsPacket.RPM = (int)this.doubleRPM;
                    }

                    World.Instance.ControlledCar.Revolution = this.characteristicsPacket.RPM;
                }

                // Braking
                else if (this.brakePedalPacket.PedalPosition > 0 && this.characteristicsPacket.Speed - (float)(0.324 / 60 * this.brakePedalPacket.PedalPosition) >= 0)
                {
                    this.characteristicsPacket.Speed -= (float)(0.324 / 60 * this.brakePedalPacket.PedalPosition) * 2; //* 2 (motion vector * 2) to ensure faster visible acceleration
                    CalculateRPM_fromSpeed(1);
                }

                // Drag
                else if (this.brakePedalPacket.PedalPosition == 0 && this.gasPedalPacket.PedalPosition == 0 && this.characteristicsPacket.Speed - (0.2) >= 0)
                {

                    this.characteristicsPacket.Speed -= (float)0.2;
                    CalculateRPM_fromSpeed(1);
                }

                else
                {
                    this.characteristicsPacket.Speed = 0;
                }
            }

            // REVERSE
            else if (this.gearboxPacket.ActualGear == OuterGear.R)
            {
                //Acceleration
                if (this.characteristicsPacket.RPM <= 3900 && this.brakePedalPacket.PedalPosition == 0 && this.gasPedalPacket.PedalPosition > 0)
                {
                    this.doubleRPM += CalculateRPMDifference(this.gasPedalPacket.PedalPosition, this.brakePedalPacket.PedalPosition, currentGearRatio);
                    this.characteristicsPacket.RPM = (int)doubleRPM;
                    World.Instance.ControlledCar.Revolution = this.characteristicsPacket.RPM;

                    if (this.characteristicsPacket.Speed > -30)
                    {
                        this.characteristicsPacket.Speed = -((this.gearboxPacket.InnerGear - 1) * 3400 + this.characteristicsPacket.RPM - 600) / 128;
                    }


                    // Debug
                    //Debug.WriteLine("Gear: " + this.virtualFunctionBus.GearboxPacket.InnerGear);
                    //Debug.WriteLine("RPM: " + this.characteristicsPacket.RPM);
                    //Debug.WriteLine("Speed: " + this.characteristicsPacket.Speed);
                    //Debug.WriteLine("fromSpeed: " + this.characteristicsPacket.RPM);
                    //Debug.WriteLine("------------------" + this.gearboxPacket.ActualGear + "------------------");
                }
                // Braking
                else if (this.brakePedalPacket.PedalPosition > 0 && this.characteristicsPacket.Speed + (float)(0.324 / 60 * this.brakePedalPacket.PedalPosition) <= 0)
                {
                    this.characteristicsPacket.Speed += (float)(0.324 / 60 * this.brakePedalPacket.PedalPosition);
                    //Debug.WriteLine(this.characteristicsPacket.Speed);
                    CalculateRPM_fromSpeed(-1);
                }
                // Drag
                else if (this.brakePedalPacket.PedalPosition == 0 && this.gasPedalPacket.PedalPosition == 0 && this.characteristicsPacket.Speed + (0.2) <= 0)
                {
                    this.characteristicsPacket.Speed += (float)0.2;
                    CalculateRPM_fromSpeed(-1);
                }
            }

            this.lastInnerGear = this.gearboxPacket.InnerGear;
        }


        private void CalculateRPM_fromSpeed(int direction)
        {
            if (direction == 1)
            {
                this.doubleRPM = (-(this.gearboxPacket.InnerGear - 1) * 3400 + 600 + 128 * this.characteristicsPacket.Speed);
                this.characteristicsPacket.RPM = (int)doubleRPM;
                World.Instance.ControlledCar.Revolution = this.characteristicsPacket.RPM;
                // Debug
                //Debug.WriteLine("Gear: " + this.virtualFunctionBus.GearboxPacket.InnerGear);
                //Debug.WriteLine("RPM: " + this.characteristicsPacket.RPM);
                //Debug.WriteLine("Speed: " + this.characteristicsPacket.Speed);
                //Debug.WriteLine("fromSpeed: " + this.characteristicsPacket.RPM);
                //Debug.WriteLine("------------------" + this.gearboxPacket.ActualGear + "------------------");
            }
            else
            {
                this.doubleRPM = (-(this.gearboxPacket.InnerGear - 1) * 3400 + 600 + 128 * -this.characteristicsPacket.Speed);
                this.characteristicsPacket.RPM = (int)doubleRPM;
                World.Instance.ControlledCar.Revolution = this.characteristicsPacket.RPM;
                // Debug
                //Debug.WriteLine("Gear: " + this.virtualFunctionBus.GearboxPacket.InnerGear);
                //Debug.WriteLine("RPM: " + this.characteristicsPacket.RPM);
                //Debug.WriteLine("Speed: " + this.characteristicsPacket.Speed);
                //Debug.WriteLine("fromSpeed: " + this.characteristicsPacket.RPM);
                //Debug.WriteLine("------------------" + this.gearboxPacket.ActualGear + "------------------");
            }   
        }
    }
}
