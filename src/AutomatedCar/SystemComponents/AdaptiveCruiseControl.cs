﻿namespace AutomatedCar.SystemComponents
{
    using AutomatedCar.Models;
    using AutomatedCar.SystemComponents.Packets;
    using Avalonia.FreeDesktop.DBusIme;
    using Newtonsoft.Json.Linq;
    using ReactiveUI;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;


    public class AdaptiveCruiseControl : SystemComponent
    {
        private const int SPEED_CHANGE_RATE = 10;

        double[] availableTargetDistances = new double[4] { 0.8, 1.0, 1.2, 1.4 };

        private int pedalPosition;

        private int selectedTargetDistanceIndex = 0;

        private AccPacket accPacket;
        private ICharacteristicsInterface carCharacteristics;

        private double deceleration = 0.1;
        private double prevDistToDetectedCar = 0.0;

        public AdaptiveCruiseControl(VirtualFunctionBus virtualFunctionBus) : base(virtualFunctionBus)
        {
            accPacket = new AccPacket();
            accPacket.IsActive = false;
            virtualFunctionBus.AccPacket = accPacket;
            carCharacteristics = virtualFunctionBus.CharacteristicsPacket;
        }

        public override void Process()
        {
            if (accPacket.IsActive)
            {
                int speed = this.SpeedIfCarInDistance();
                this.pedalPosition = CalculatePedalPosition(speed < this.accPacket.SelectedSpeed ? speed : accPacket.SelectedSpeed, carCharacteristics.Speed);
                if (this.pedalPosition < 0)
                {
                    virtualFunctionBus.BrakePedalPacket.PedalPosition = (byte)Math.Abs(pedalPosition);
                    virtualFunctionBus.GasPedalPacket.PedalPosition = 0;

                }
                else
                {
                    virtualFunctionBus.GasPedalPacket.PedalPosition = (byte)pedalPosition;
                    virtualFunctionBus.BrakePedalPacket.PedalPosition = 0;
                }
                accPacket.Distance = CalcualteFollowDistance();
            }
            carCharacteristics = virtualFunctionBus.CharacteristicsPacket;
            virtualFunctionBus.AccPacket = this.accPacket;
        }

        public static int CalculatePedalPosition(double targetSpeed, double currentSpeed)
        {
            double speedDifference = targetSpeed - currentSpeed;
            double maxPedalPosition = 100.0;
            double pedalPosition = speedDifference / maxPedalPosition;

            double threshold = 40.0; // Speed difference threshold, if the difference is big enough this gives more umpf for the pedals
            if (pedalPosition > 0 && speedDifference > threshold)
            {
                pedalPosition += (speedDifference - threshold) / maxPedalPosition;
            }
            else if (pedalPosition < 0 && speedDifference < -threshold)
            {
                pedalPosition += (speedDifference - threshold) / maxPedalPosition;
            }

            pedalPosition = Math.Clamp(pedalPosition, -1.0, 1.0);
            int pedalPositionInt = (int)(pedalPosition * 100.0);

            return pedalPositionInt;
        }

        internal void ToggleCruiseControl()
        {
            accPacket.IsActive = !accPacket.IsActive;

            var selectedSpeed = (int)carCharacteristics.Speed;

            if (selectedSpeed < 30)
            {
                selectedSpeed = 30;
            }
            else if(selectedSpeed > 160)
            {
                selectedSpeed = 160;
            }

            if (!accPacket.IsActive)
            {

                ResetAcc();
            }
            else
            {
                accPacket.SelectedTargetDistance = availableTargetDistances[selectedTargetDistanceIndex];
                accPacket.SelectedSpeed = selectedSpeed;
            }

            virtualFunctionBus.AccPacket = accPacket;
        }

        private void ResetAcc()
        {
            accPacket.SelectedTargetDistance = 0;
            accPacket.SelectedSpeed = 0;
            selectedTargetDistanceIndex = 0;

        }

        internal void DecreaseTargetSpeed()
        {
            if (accPacket.SelectedSpeed != 0) 
            {
                if (accPacket.SelectedSpeed == 30)
                    return;

                accPacket.SelectedSpeed = (int)(Math.Floor((double)accPacket.SelectedSpeed / 10) * 10) - SPEED_CHANGE_RATE;
            }
        }

        internal void IncreaseTargetSpeed()
        {
            if (accPacket.SelectedSpeed != 0)
            {

                if (accPacket.SelectedSpeed == 160)
                    return;

                accPacket.SelectedSpeed = (int)(Math.Floor((double)accPacket.SelectedSpeed / 10) * 10) + SPEED_CHANGE_RATE;
            }
        }

        internal void ChangeTargetDistance()
        {
            selectedTargetDistanceIndex++;
            accPacket.SelectedTargetDistance = availableTargetDistances[selectedTargetDistanceIndex % availableTargetDistances.Length];
        }

        public void SetIsActiveFalse()
        { 
            accPacket.IsActive = false;
            ResetAcc();
        }

        internal int SpeedIfCarInDistance()
        {
            var obj = this.SelectCar();
            if (obj != null && obj.DetectedObject.Filename == "car_3_black.png" && obj.Distance / 50 < 100)
            {
                return RequiredSpeed(obj.Distance / 50, carCharacteristics.Speed, (float)this.availableTargetDistances[selectedTargetDistanceIndex % availableTargetDistances.Length]);
            }

            this.prevDistToDetectedCar = 0.0;
            return accPacket.SelectedSpeed;
        }

        internal DetectedObjectInfo SelectCar()
        {
            return this.virtualFunctionBus.RadarPacket.WorldObjectsDetected.Where(x => x.DetectedObject.WorldObjectType == WorldObjectType.Other).FirstOrDefault();
        }

        internal int RequiredSpeed(double distanceToDetectedCar, double controlledCarSpeed, float followDistance)
        {
            double detectedCarSpeed = this.CalculateDetectedCarSpeed(distanceToDetectedCar, controlledCarSpeed);
            double requiredDistance = this.CalculateRequiredDistance(detectedCarSpeed, followDistance);
            if (requiredDistance > distanceToDetectedCar && controlledCarSpeed > detectedCarSpeed)
            {
                double requiredSpeed = this.CalculateRequiredSpeed(this.deceleration, controlledCarSpeed, followDistance);
                return (int)requiredSpeed;
            }
            return accPacket.SelectedSpeed;
        }

        private double CalculateDetectedCarSpeed(double distanceToDetectedCar, double controlledCarSpeed)
        {
            if (this.prevDistToDetectedCar == 0.0)
            {
                this.prevDistToDetectedCar = distanceToDetectedCar;
            }

            double distanceChange = this.prevDistToDetectedCar - distanceToDetectedCar;
            if (distanceChange == 0)
            {
                return controlledCarSpeed;
            }

            double distanceChangeKm = distanceChange / 1000;
            double controlledCarSpeedMps = controlledCarSpeed / 3.6;
            double detectedCarSpeed = Math.Abs((distanceChangeKm / (1.0 / 60)) - controlledCarSpeedMps);

            return detectedCarSpeed;
        }

        private double CalculateRequiredDistance(double detectedCarSpeed, float followDistance)
        {
            double requiredDistance = detectedCarSpeed * followDistance;

            return requiredDistance;
        }

        private double CalculateRequiredSpeed(double deceleration, double controlledCarSpeed, float followDistance)
        {
            double requiredSpeed = controlledCarSpeed - (deceleration * followDistance);

            return requiredSpeed;
        }

        internal double CalcualteFollowDistance()
        {
            var obj = this.SelectCar();
            if (obj == null)
                return 0.0;
            return Math.Round((obj.Distance / 50) / (carCharacteristics.Speed / 3.6), 1);
        }
    }
}
