﻿namespace AutomatedCar.SystemComponents.Packets
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public interface IGearboxInterface
    {
        bool ShiftInProgress { get; }
        byte InnerGear { get; }
        byte OuterGear { get; }
    }
}
