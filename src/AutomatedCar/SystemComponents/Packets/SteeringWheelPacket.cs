namespace AutomatedCar.SystemComponents.Packets
{
    using AutomatedCar.Models;
    using ReactiveUI;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class SteeringWheelPacket : ReactiveObject, ISteeringWheel
    {
        private Vector2D vector2D;

        public Vector2D DirectionVector
        {
            get => this.vector2D;
            set => this.RaiseAndSetIfChanged(ref this.vector2D, value);
        }
    }
}
