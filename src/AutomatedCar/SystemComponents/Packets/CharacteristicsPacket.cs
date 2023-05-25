namespace AutomatedCar.SystemComponents.Packets
{
    using ReactiveUI;

    public class CharacteristicsPacket: ReactiveObject, ICharacteristicsInterface
    {
        private int rPM;
        private float speed;

        public int RPM { get => this.rPM; set => this.RaiseAndSetIfChanged(ref this.rPM, value); }

        public float Speed { get => this.speed; set => this.RaiseAndSetIfChanged(ref this.speed, value); }
    }
}
