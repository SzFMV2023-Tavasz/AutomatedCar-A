namespace AutomatedCar.SystemComponents.Packets
{
    using ReactiveUI;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class GearBoxPacket : ReactiveObject, IGearboxInterface
    {

        private bool shiftInProgress;

        private byte innerGear;


        public bool ShiftInProgress { get;  }

        public byte InnerGear { get; }
    }
}
