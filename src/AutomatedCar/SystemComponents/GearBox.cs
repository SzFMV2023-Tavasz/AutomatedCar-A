namespace AutomatedCar.SystemComponents
{
    using AutomatedCar.Models;
    using AutomatedCar.SystemComponents.Packets;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class GearBox : SystemComponent
    {

        public GearBoxPacket gearBoxPacket = new GearBoxPacket();
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
                Shift(1);
            }
            else if (this.characteristicsPacket.RPM < 1400 && this.gearBoxPacket.InnerGear > 1)
            {
                Shift(-1);
            }
        }


        /// <summary>
        /// 1 means up, -1 means down
        /// </summary>
        public void Shift(int upOrDown)
        {
            this.aTimer.Start();
            while (this.aTimer.Enabled)
            {
                this.gearBoxPacket.ShiftInProgress = true;
            }

            if (upOrDown == 1)
            {
                this.gearBoxPacket.InnerGear++;
            }
            else
            {
                this.gearBoxPacket.InnerGear--;
            }
            this.gearBoxPacket.ShiftInProgress = false;

        }
    }
}
