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

        public bool AEBIsActive
        {
            get => this._AEBIsActive;
            set => this.RaiseAndSetIfChanged(ref this._AEBIsActive, value);
        }
    }
}