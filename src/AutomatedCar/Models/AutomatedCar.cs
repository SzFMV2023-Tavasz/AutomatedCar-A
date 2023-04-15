namespace AutomatedCar.Models
{
    using Avalonia.Data;
    using Avalonia.Media;
    using System.Collections.Generic;
    using SystemComponents;

    public class AutomatedCar : Car
    {
        private VirtualFunctionBus virtualFunctionBus;

        public AutomatedCar(int x, int y, string filename)
            : base(x, y, filename)
        {
            this.virtualFunctionBus = new VirtualFunctionBus();
            this.ZIndex = 10;
            this.RadarSensor = new RadarSensor(this, this.virtualFunctionBus, new Avalonia.Point(54,0), 60, 50, new List<WorldObjectType>() {
                WorldObjectType.Boundary,
                WorldObjectType.Building,
                WorldObjectType.Car,
                WorldObjectType.Crosswalk,
                WorldObjectType.Other,
                WorldObjectType.ParkingSpace,
                WorldObjectType.Pedestrian,
                WorldObjectType.Road,
                WorldObjectType.RoadSign,
                WorldObjectType.Tree, });
        }

        public RadarSensor RadarSensor { get;  }

        public VirtualFunctionBus VirtualFunctionBus { get => this.virtualFunctionBus; }

        public int Revolution { get; set; }

        public int Velocity { get; set; }

        public PolylineGeometry Geometry { get; set; }

        /// <summary>Starts the automated cor by starting the ticker in the Virtual Function Bus, that cyclically calls the system components.</summary>
        public void Start()
        {
            this.virtualFunctionBus.Start();
        }

        /// <summary>Stops the automated cor by stopping the ticker in the Virtual Function Bus, that cyclically calls the system components.</summary>
        public void Stop()
        {
            this.virtualFunctionBus.Stop();
        }
    }
}