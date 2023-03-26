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

        public Vector2 CarAnchorPoint { get => carAnchorPoint; set => carAnchorPoint = value; }
        public double FOV { get => fov; set => fov = value; }
        public double ViewDistance { get => viewDistance; set => viewDistance = value; }
        public IEnumerable<WorldObjectType> WorldObjectTypesFilter { get => worldObjectTypesFilter; set => worldObjectTypesFilter = value; }
        IReadOnlyPacket Packet { get => packet; set => packet = value; }

        protected Vector2 carAnchorPoint;
        protected double fov;
        protected double viewDistance;
        protected IEnumerable<WorldObjectType> worldObjectTypesFilter;
        protected IReadOnlyPacket packet;
    }
}
