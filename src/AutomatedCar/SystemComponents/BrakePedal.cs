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
        public bool isPedalPressed;
        private BrakePedalPacket brakePedalPacket;
        private IAEBInterface aeb;

        public BrakePedal(VirtualFunctionBus virtualFunctionBus) : base(virtualFunctionBus)
        {
            this.brakePedalPacket = new BrakePedalPacket();
            virtualFunctionBus.BrakePedalPacket = this.brakePedalPacket;
            this.aeb = virtualFunctionBus.AEBPacket;
        }

        public override void Process()
        {
            this.aeb = this.virtualFunctionBus.AEBPacket;

            if (this.aeb.AEBIsActive)
            {
                this.brakePedalPacket.PedalPosition = 100;
                isPedalPressed = true;
            }

            if (this.isPedalPressed)
            {
                PressBrakePedal();
            }

            else
            {
                if (this.isPedalPressed == false && this.brakePedalPacket.PedalPosition != 0)
                {
                    this.brakePedalPacket.PedalPosition -= 2;
                }
            }

            if (!Keyboard.Keys.Contains(Avalonia.Input.Key.Down))
            {
                this.isPedalPressed = false;
            }
        }

        public void PressBrakePedal()
        {
            if (!this.aeb.AEBIsActive)
            {
                isPedalPressed = true;

                if (this.brakePedalPacket.PedalPosition < 99)
                {
                    this.brakePedalPacket.PedalPosition += 2;
                }
            }
        }
    }
}