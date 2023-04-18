namespace AutomatedCar.SystemComponents.Packets
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    public enum OuterGear
    {
        p = 0,
        r = 1,
        n = 2,
        d = 3
    }
    public interface IGearboxInterface
    {
        bool ShiftInProgress { get; }
        byte InnerGear { get; }
        /// <summary>
        /// enum: P, R, N, D
        /// </summary>
        OuterGear ActualGear { get; }
    }
}
