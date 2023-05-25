namespace AutomatedCar.SystemComponents.Packets
{
    using ReactiveUI;
    using System.Numerics;

    public class DrivechainPacket : ReactiveObject, IDrivechain
    {
        private int speed;
        private Vector2 motionVector;
        private float VectorDifferentialLength;
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

        public float vectorDifferentialLength
        {
            get => this.VectorDifferentialLength;
            set => this.RaiseAndSetIfChanged(ref this.VectorDifferentialLength, value);
        }
    }
}