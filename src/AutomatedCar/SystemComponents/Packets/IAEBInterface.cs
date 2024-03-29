﻿namespace AutomatedCar.SystemComponents.Packets
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public interface IAEBInterface
    {
        public bool AEBIsActive { get; }

        public double BreakWarning { get; }

        public double YellowWarning { get; }

        public double RedWarning { get; }
    }
}
