namespace AutomatedCar.SystemComponents.Packets
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    public enum OuterGear
    {
        P = 0,
        R = 1,
        N = 2,
        D = 3
    }
    public interface IGearboxInterface
    {
        bool ShiftInProgress { get; }
        byte InnerGear { get; }
        /// <summary>
        /// enum: P, R, N, D
        /// </summary>
        OuterGear ActualGear { get; }
        string PreviousGear { get; }
        string NextGear { get; }
    }
}
