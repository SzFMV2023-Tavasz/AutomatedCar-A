namespace AutomatedCar.SystemComponents.Packets
{
    using ReactiveUI;

    public class BrakePedalPacket : ReactiveObject, IPedalInterface
    {
        private byte pedalPosition;

        public byte PedalPosition
        {
            get => this.pedalPosition;
            set => this.RaiseAndSetIfChanged(ref this.pedalPosition, value);
        }
    }
}