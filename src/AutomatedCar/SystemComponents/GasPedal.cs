namespace AutomatedCar.SystemComponents
{
    using AutomatedCar.Models;
    using AutomatedCar.SystemComponents.Packets;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class GasPedal: SystemComponent
    {
        private bool isPedalPressed;
        private GasPedalPacket gasPedalPacket;

        public GasPedal(VirtualFunctionBus virtualFunctionBus): base(virtualFunctionBus)
        {
            this.gasPedalPacket = new GasPedalPacket();
            virtualFunctionBus.GasPedalPacket = this.gasPedalPacket;
        }

        public override void Process()
        {
            if (this.gasPedalPacket.PedalPosition > 0 && isPedalPressed == false)
            {
                this.gasPedalPacket.PedalPosition -= 2;
            }

            isPedalPressed = false;
        }

        public void PressGasPedal()
        {
            isPedalPressed = true;

            if (this.gasPedalPacket.PedalPosition < 98)
            {
                this.gasPedalPacket.PedalPosition += 2;
            }
        }
    }
}