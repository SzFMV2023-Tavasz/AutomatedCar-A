namespace AutomatedCar.ViewModels
{
    using AutomatedCar.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class CarViewModel : WorldObjectViewModel
    {
        public AutomatedCar Car { get; set; }
        public DetectedObjectInfo Camera { get => Car.VirtualFunctionBus.CameraPacket.WorldObjectsDetected?.OrderBy(x=>x.Distance).FirstOrDefault(); }
        public DetectedObjectInfo Radar { get => Car.VirtualFunctionBus.RadarPacket.WorldObjectsDetected?.Aggregate((min, next) => next.Distance < min.Distance ? next : min); }

        public string LKAinfo { get => Car.VirtualFunctionBus.LKAStatusPacket.Status; }
        public CarViewModel(AutomatedCar car) : base(car)
        {
            this.Car = car;
        }
    }
}