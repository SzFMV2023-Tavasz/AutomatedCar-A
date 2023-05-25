namespace AutomatedCar.SystemComponents.Packets
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Numerics;
    using System.Text;
    using System.Threading.Tasks;

    public interface IDrivechain
    {
        public int Speed { get; }

        public Vector2 MotionVector { get; }

        public float vectorDifferentialLength { get; }
    }
}