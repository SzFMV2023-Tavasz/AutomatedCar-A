namespace AutomatedCar.SystemComponents.Packets
{
    using ReactiveUI;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Numerics;
    using System.Text;
    using System.Threading.Tasks;

    public class AEBPacket : ReactiveObject, IAEBInterface
    {
        private bool _AEBIsActive;
        private double breakWarning = 0;
        private double yellowWarning = 0;
        private double redWarning = 0;

        public bool AEBIsActive
        {
            get => this._AEBIsActive;
            set => this.RaiseAndSetIfChanged(ref this._AEBIsActive, value);
        }

        public double BreakWarning
        {
            get => this.breakWarning;
            set => this.RaiseAndSetIfChanged(ref this.breakWarning, value);
        }

        public double YellowWarning
        {
            get => this.yellowWarning;
            set => this.RaiseAndSetIfChanged(ref this.yellowWarning, value);
        }

        public double RedWarning
        {
            get => this.redWarning;
            set => this.RaiseAndSetIfChanged(ref this.redWarning, value);
        }
    }
}
