namespace AutomatedCar.Models
{
    using SystemComponents;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Avalonia;

    public class SensorSettings
    {
        public AutomatedCar Car { get; set; }

        public VirtualFunctionBus FunctionBus { get; set; }

        public Point CarAnchorPoint { get;set; }

        public double FieldOfView { get; set; }

        public double ViewDistance { get; set; }

        public IEnumerable<WorldObjectType> WorldObjectFilter { get; set; }
    }
}