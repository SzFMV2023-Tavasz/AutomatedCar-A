namespace AutomatedCar.SystemComponents.Packets
{
    using ReactiveUI;
    using System.Numerics;

    public class DrivechainPacket : ReactiveObject, IDrivechain
    {
        private int speed;
        private Vector2 motionVector;

        public int Speed
        {
            get => this.speed;
            set => this.RaiseAndSetIfChanged(ref this.speed, value);
        }

        public Vector2 MotionVector
        {
            get => this.motionVector;
            set => this.RaiseAndSetIfChanged(ref this.motionVector, value);
        }
    }
}