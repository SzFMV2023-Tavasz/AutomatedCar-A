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
        private IDrivechain drivechainPacket;

        public GearBox(VirtualFunctionBus virtualFunctionBus) : base(virtualFunctionBus)
        {
            virtualFunctionBus.GearboxPacket = this.gearBoxPacket;
            this.gearBoxPacket.ShiftInProgress = false;
            this.gearBoxPacket.InnerGear = 1;
            this.gearBoxPacket.ActualGear = OuterGear.P;
            this.gearBoxPacket.PreviousGear = "";
            this.gearBoxPacket.NextGear = OuterGear.R.ToString();
        }

        public override void Process()
        {
            this.characteristicsPacket = World.Instance.ControlledCar.VirtualFunctionBus.CharacteristicsPacket;
            this.drivechainPacket = World.Instance.ControlledCar.VirtualFunctionBus.drivechainPacket;

            if (this.gearBoxPacket.InnerGear < 5 && this.characteristicsPacket.RPM >= 4000 && this.gearBoxPacket.ActualGear != OuterGear.N)
            {
                Shift(1);
                this.gearBoxPacket.ShiftInProgress = false;
            }
            else if (this.gearBoxPacket.InnerGear > 1 && this.characteristicsPacket.RPM <= 600)
            {
                Shift(-1);
                this.gearBoxPacket.ShiftInProgress = false;
            }
        }


        /// <summary>
        /// 1 means up, -1 means down
        /// wait 1 second, and than 
        /// </summary>
        
        public async void Shift(int upOrDown)
        {
            this.gearBoxPacket.ShiftInProgress = true;
            if (upOrDown == 1)
            {
                this.gearBoxPacket.InnerGear++;
            }
            else
            {
                this.gearBoxPacket.InnerGear--;
            }

            await Task.Delay(1000);
        }

        public void OuterGearShiftUp()
        {
            if (this.gearBoxPacket.ActualGear != OuterGear.D && this.drivechainPacket.Speed == 0)
            {
                this.gearBoxPacket.PreviousGear = gearBoxPacket.ActualGear.ToString();
                this.gearBoxPacket.ActualGear++;

                if (this.gearBoxPacket.ActualGear is OuterGear.D)
                {
                    this.gearBoxPacket.NextGear = string.Empty;
                }
                else
                {
                    this.gearBoxPacket.NextGear = (this.gearBoxPacket.ActualGear + 1).ToString();
                }
            }
        }

        public void OuterGearShiftDown()
        {
            if (this.gearBoxPacket.ActualGear != OuterGear.P && this.drivechainPacket.Speed == 0)
            {
                this.gearBoxPacket.NextGear = this.gearBoxPacket.ActualGear.ToString();
                this.gearBoxPacket.ActualGear--;
            }

            if (this.gearBoxPacket.ActualGear is OuterGear.P)
            {
                this.gearBoxPacket.PreviousGear = string.Empty;
            }
            else
            {
                this.gearBoxPacket.PreviousGear = (this.gearBoxPacket.ActualGear - 1).ToString();
            }
        }
    }
}