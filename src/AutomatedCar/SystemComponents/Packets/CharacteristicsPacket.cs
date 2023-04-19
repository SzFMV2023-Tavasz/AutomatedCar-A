using ReactiveUI;
using System;
namespace AutomatedCar.SystemComponents.Packets
{
    public class CharacteristicsPacket: ReactiveObject, ICharacteristicsInterface
    {
        private int rPM = 1200;
        private float speed;

        public int RPM { get => this.rPM; set => this.RaiseAndSetIfChanged(ref this.rPM, value); }

        public float Speed { get => this.speed; set => this.RaiseAndSetIfChanged(ref this.speed, value); }

    }
}