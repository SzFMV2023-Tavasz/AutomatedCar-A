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


        public bool ShiftInProgress {
            get => this.shiftInProgress;
            set => this.RaiseAndSetIfChanged(ref this.shiftInProgress, value);
        }

        public byte InnerGear {

            get => this.innerGear;
            set => this.RaiseAndSetIfChanged(ref this.innerGear, value);

        }
    }
}
