namespace AutomatedCar.SystemComponents.Packets
{
    using ReactiveUI;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    internal class LKAPacket : ReactiveObject, ILaneKeepingPacket
    {
        public bool isEnabled { get; set ; }
        public bool canBeEnabled { get; set; }
        public double recommendedTurnAngle { get; set; }
    }
}
