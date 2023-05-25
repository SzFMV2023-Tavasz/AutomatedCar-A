namespace AutomatedCar.SystemComponents.Packets
{
    using AutomatedCar.Models;
    using ReactiveUI;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class ReadOnlyPacket : ReactiveObject, IReadOnlyPacket<DetectedObjectInfo>
    {
        private IReadOnlyCollection<DetectedObjectInfo> worldObjectsDetected;


        public IReadOnlyCollection<DetectedObjectInfo> WorldObjectsDetected
        {
            get => this.worldObjectsDetected;
            set => this.RaiseAndSetIfChanged(ref this.worldObjectsDetected, value);
        }
    }
}
