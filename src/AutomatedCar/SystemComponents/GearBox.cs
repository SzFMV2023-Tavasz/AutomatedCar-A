namespace AutomatedCar.SystemComponents
{
    using AutomatedCar.Models;
    using AutomatedCar.SystemComponents.Packets;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class GearBox :SystemComponent
    {

        private GearBoxPacket gearBoxPacket = new GearBoxPacket();
        private ICharacteristicsInterface characteristicsPacket;

        System.Timers.Timer aTimer = new System.Timers.Timer(2000);



        public GearBox(VirtualFunctionBus virtualFunctionBus) : base(virtualFunctionBus)
        {
            virtualFunctionBus.GearboxPacket = this.gearBoxPacket;
            this.gearBoxPacket.ShiftInProgress = false;
            this.gearBoxPacket.InnerGear = 1;

        }


        /// <summary>
        /// TODO
        /// </summary>
        public override void Process()
        {
            this.characteristicsPacket = World.Instance.ControlledCar.VirtualFunctionBus.CharacteristicsPacket;

            if (this.characteristicsPacket.RPM > 2500 && this.gearBoxPacket.InnerGear < 5)
            {
                this.aTimer.Start();
                while (this.aTimer.Enabled)
                {
                    this.gearBoxPacket.ShiftInProgress = true;
                }

                Shift(1);

                this.gearBoxPacket.ShiftInProgress = false;
            }
        }

        private void Shift(byte upOrDown)
        {
            this.gearBoxPacket.InnerGear += upOrDown;
        }
    }
}
