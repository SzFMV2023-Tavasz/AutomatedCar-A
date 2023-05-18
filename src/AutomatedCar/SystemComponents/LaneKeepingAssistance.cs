namespace AutomatedCar.SystemComponents
{
    using AutomatedCar.SystemComponents.Packets;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    internal class LaneKeepingAssistance : SystemComponent
    {
        private LKAPacket packet;

        public LaneKeepingAssistance(VirtualFunctionBus virtualFunctionBus):base(virtualFunctionBus)
        {

            this.packet= new LKAPacket();
            packet.canBeEnabled=true;
            packet.isEnabled=false;
            packet.recommendedTurnAngle=0;
            virtualFunctionBus.LaneKeepingPacket= packet;
        }
        public override void Process()
        {
            throw new NotImplementedException();
        }
    }
}
