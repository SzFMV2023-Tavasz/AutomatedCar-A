namespace AutomatedCar.SystemComponents.Packets
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public interface ILaneKeepingPacket
    {
        
        bool isEnabled { get; set; }
        bool canBeEnabled { get; set; }

        public double recommendedTurnAngle { get; set; }
    }
}
