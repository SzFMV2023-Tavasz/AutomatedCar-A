namespace AutomatedCar.SystemComponents.Packets
{
    using ReactiveUI;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class GearBoxPacket : ReactiveObject, IGearboxPacket
    {

        private bool shiftInProgress;
        private byte innerGear;


        public bool ShiftInProgress {

            get { return shiftInProgress; }
        }

        public byte InnerGear
        {
            get { return innerGear; }
        }
    }
}
