namespace AutomatedCar.SystemComponents.Packets
{
    using ReactiveUI;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class LKAInfoPacket : ReactiveObject, ILKAInfoPacket
    {
        public string Status { get => _status; set => this.RaiseAndSetIfChanged(ref _status, value); }
        private string _status;
    }
}
