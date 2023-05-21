namespace AutomatedCar.SystemComponents
{
    using AutomatedCar.SystemComponents.Packets;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class LaneKeepingAssistance : SystemComponent
    {
        private LKAPacket packet;

        public bool isEnabled { get; set; }
        public bool canBeEnabled()
        {
            // egyelore csak
            // hogy buildeljen
            return true;

        }

        public LaneKeepingAssistance(VirtualFunctionBus virtualFunctionBus):base(virtualFunctionBus)
        {

            this.packet= new LKAPacket();

            packet.recommendedTurnAngle=0;
            virtualFunctionBus.LaneKeepingPacket= packet;
        }
        public override void Process()
        {
            throw new NotImplementedException();
        }
    }
}
