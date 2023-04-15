using ReactiveUI;
using System;
namespace AutomatedCar.SystemComponents.Packets
{
    
    public class CharacteristicsPacket: ReactiveObject
    {
        private int rPM;
        public int RPM { get => this.rPM; set => this.RaiseAndSetIfChanged(ref this.rPM, value); }
    }
}

