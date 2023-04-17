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
            SensorSettings radarSettings = new SensorSettings()
            {
                Car = this,
                FunctionBus = this.virtualFunctionBus,
                CarAnchorPoint = new Avalonia.Point(54, 0),
                FieldOfView = 60,
                ViewDistance = 200,
                WorldObjectFilter = new List<WorldObjectType>() {
                    WorldObjectType.Boundary,
                    WorldObjectType.Building,
                    WorldObjectType.Car,
                    WorldObjectType.Other,
                    WorldObjectType.Pedestrian,
                    WorldObjectType.Tree, },
            };
            SensorSettings cameraSettings = new SensorSettings()
            {
                Car = this,
                FunctionBus = this.virtualFunctionBus,
                CarAnchorPoint = new Avalonia.Point(54, 74),
                FieldOfView = 60,
                ViewDistance = 80,
                WorldObjectFilter = new List<WorldObjectType>() {
                    WorldObjectType.Boundary,
                    WorldObjectType.Crosswalk,
                    WorldObjectType.Road,
                    WorldObjectType.RoadSign,
                    WorldObjectType.ParkingSpace, },
            };
            this.RadarSensor = new RadarSensor(radarSettings);
            this.CameraSensor = new CameraSensor(cameraSettings);
            this.CollSensor = new CollisionSensor(virtualFunctionBus,this);
        }

        public RadarSensor RadarSensor { get;  }

        public CameraSensor CameraSensor { get; }
        public CollisionSensor CollSensor { get; }

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