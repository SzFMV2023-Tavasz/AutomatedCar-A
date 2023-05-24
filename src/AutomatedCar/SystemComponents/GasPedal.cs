namespace AutomatedCar.SystemComponents
{
    using AutomatedCar.Models;
    using AutomatedCar.SystemComponents.Packets;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class GasPedal: SystemComponent
    {
        public bool isPedalPressed;
        private GasPedalPacket gasPedalPacket;
        private IAEBInterface aeb; 

        public GasPedal(VirtualFunctionBus virtualFunctionBus): base(virtualFunctionBus)
        {
            this.gasPedalPacket = new GasPedalPacket();
            virtualFunctionBus.GasPedalPacket = this.gasPedalPacket;
            this.aeb = virtualFunctionBus.AEBPacket;
        }

        public override void Process()
        {
            this.aeb = this.virtualFunctionBus.AEBPacket;

            if (this.aeb.AEBIsActive)
            {
                this.isPedalPressed = false;
                this.gasPedalPacket.PedalPosition = 0;
            }

            if (this.isPedalPressed)
            {
                PressGasPedal();
            }

            else
            {
                if (this.isPedalPressed == false && this.gasPedalPacket.PedalPosition != 0)
                {
                    this.gasPedalPacket.PedalPosition -= 2;
                }
            }

            if (!Keyboard.Keys.Contains(Avalonia.Input.Key.Up))
            {
                this.isPedalPressed = false;
            }
        }

        public void PressGasPedal()
        {
            if (!this.aeb.AEBIsActive)
            {
                this.isPedalPressed = true;

                if (this.gasPedalPacket.PedalPosition < 100)
                {
                    this.gasPedalPacket.PedalPosition += 2;
                }
            }
        }
    }
}