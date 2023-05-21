namespace AutomatedCar.SystemComponents
{
    using AutomatedCar.Models;
    using AutomatedCar.SystemComponents.Packets;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class BrakePedal : SystemComponent
    {
        private bool isPedalPressed;
        private BrakePedalPacket brakePedalPacket;

        public BrakePedal(VirtualFunctionBus virtualFunctionBus) : base(virtualFunctionBus)
        {
            this.brakePedalPacket = new BrakePedalPacket();
            virtualFunctionBus.BrakePedalPacket = this.brakePedalPacket;
        }

        public override void Process()
        {
            if (this.brakePedalPacket.PedalPosition > 0 && isPedalPressed == false)
            {
                this.brakePedalPacket.PedalPosition -= 2;
            }
            isPedalPressed = false;
        }

        public void PressBrakePedal()
        {
            isPedalPressed = true;
            if (this.brakePedalPacket.PedalPosition < 99)
            {
                this.brakePedalPacket.PedalPosition += 2;
            }
        }
    }
}

