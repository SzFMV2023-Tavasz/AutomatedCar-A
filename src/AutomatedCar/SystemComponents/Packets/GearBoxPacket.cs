namespace AutomatedCar.SystemComponents.Packets
{
    using ReactiveUI;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection.Metadata.Ecma335;
    using System.Text;
    using System.Threading.Tasks;

    public class GearBoxPacket : ReactiveObject, IGearboxInterface
    {

        private bool shiftInProgress;
        private string previousGear;
        private string nextGear;
        private byte innerGear;
        private OuterGear actualGear;


        public bool ShiftInProgress
        {
            get => this.shiftInProgress;
            set => this.RaiseAndSetIfChanged(ref this.shiftInProgress, value);
        }

        public byte InnerGear
        {

            get => this.innerGear;
            set => this.RaiseAndSetIfChanged(ref this.innerGear, value);
        }

        public OuterGear ActualGear
        {
            get => this.actualGear;
            set => this.RaiseAndSetIfChanged(ref this.actualGear, value);
        }

        public string PreviousGear
        {
            get => this.previousGear;
            set => this.RaiseAndSetIfChanged(ref this.previousGear, value);
        }

        public string NextGear
        {
            get => this.nextGear;
            set => this.RaiseAndSetIfChanged(ref this.nextGear, value);
        }
    }
}