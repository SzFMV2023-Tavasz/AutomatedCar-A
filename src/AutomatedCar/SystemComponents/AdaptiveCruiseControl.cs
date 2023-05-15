namespace AutomatedCar.SystemComponents
{
    using AutomatedCar.SystemComponents.Packets;
    using Newtonsoft.Json.Linq;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    internal class AdaptiveCruiseControl : SystemComponent
    {
        private int pedalPosition;
        double targetSpeed;
        double currentSpeed;

        public AdaptiveCruiseControl(VirtualFunctionBus virtualFunctionBus) : base(virtualFunctionBus)
        {
        }

        public override void Process()
        {
            this.pedalPosition = CalculatePedalPosition(this.targetSpeed, this.currentSpeed);
            if (this.pedalPosition < 0)
            {
                // TODO - Fékpedál értéke (byte)Math.Abs(pedalPosition)
            }
            else
            {
                // TODO - Gázpedál értéke (byte)pedalPosition
            }
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
    }
}
