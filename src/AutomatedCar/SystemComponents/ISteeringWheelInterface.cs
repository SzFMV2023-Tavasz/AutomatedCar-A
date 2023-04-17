namespace AutomatedCar.SystemComponents
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Numerics;
    using System.Text;
    using System.Threading.Tasks;
    using AutomatedCar.Models;

    public interface ISteeringWheelInterface
    {
        Vector2D DirectionVector { get; }
    }
}
