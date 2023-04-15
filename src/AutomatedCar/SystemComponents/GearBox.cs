namespace AutomatedCar.SystemComponents
{
    using AutomatedCar.SystemComponents.Packets;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class GearBox :SystemComponent
    {

        GearBoxPacket gearBoxPacket = new GearBoxPacket();

        int _RPM;

        System.Timers.Timer aTimer = new System.Timers.Timer();
        
        //aTimer.Interval = 5000;
        //aTimer.Enabled = true;



        public GearBox(VirtualFunctionBus virtualFunctionBus, int timerInterval) : base(virtualFunctionBus)
        {
            virtualFunctionBus.GearboxPacket=gearBoxPacket;
            this._RPM = virtualFunctionBus.CharacteristicsPacket.RPM;
            aTimer.Interval= timerInterval;
            
        }

        public override void Process()
        {
            //ToDo
        }

        private void Shift()
        {

        }
    }
}
