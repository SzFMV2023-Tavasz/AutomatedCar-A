namespace AutomatedCar.SystemComponents
{
    using AutomatedCar.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Numerics;
    using System.Text;
    using System.Threading.Tasks;

    public abstract class GenericSensor : SystemComponent
    {


        public GenericSensor(VirtualFunctionBus virtualFunctionBus,
                    Vector2 carAnchorPoint,
                    double fOV, double viewDistance,
                    IEnumerable<WorldObjectType> worldObjectTypesFilter)
                        : base(virtualFunctionBus)
        {
            CarAnchorPoint = carAnchorPoint;
            FOV = fOV;
            ViewDistance = viewDistance;
            WorldObjectTypesFilter = worldObjectTypesFilter;
        }

        public Vector2 CarAnchorPoint { get; set; }
        public double FOV { get; set; }
        public double ViewDistance { get; set; }
        public IEnumerable<WorldObjectType> WorldObjectTypesFilter { get; set; }
        IReadOnlyPacket Packet { get; set; }

        //protected Vector2 carAnchorPoint;
        //protected double fov;
        //protected double viewDistance;
        //protected IEnumerable<WorldObjectType> worldObjectTypesFilter;
        //protected IReadOnlyPacket packet;
    }
}
