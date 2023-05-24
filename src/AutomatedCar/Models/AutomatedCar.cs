using AutomatedCar.SystemComponents;

namespace AutomatedCar.Models
{
    using Avalonia.Data;
    using Avalonia.Media;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.Collections.Generic;
    using SystemComponents;

    public class AutomatedCar : Car, INotifyPropertyChanged
    {
        private VirtualFunctionBus virtualFunctionBus;
        public GasPedal GasPedal;
        public event PropertyChangedEventHandler PropertyChanged;

        private GearBox gearBox;
        private Drivechain drivechain;

        public SteeringWheel steeringWheel;

        public BrakePedal BrakePedal;
        public AdaptiveCruiseControl CruiseControl;
        private int velo;
        private int revo;

        public Characteristics characteristics;

        public AutomatedCar(int x, int y, string filename)
            : base(x, y, filename)
        {
            this.virtualFunctionBus = new VirtualFunctionBus();
            this.gearBox = new GearBox(this.virtualFunctionBus);
            this.steeringWheel = new SteeringWheel(this.virtualFunctionBus);
            this.BrakePedal = new BrakePedal(this.virtualFunctionBus);
            this.GasPedal = new GasPedal(this.virtualFunctionBus);

            this.CruiseControl = new AdaptiveCruiseControl(this.virtualFunctionBus);

            this.characteristics = new Characteristics(this.virtualFunctionBus);
            this.drivechain = new Drivechain(this.virtualFunctionBus);
            this.ZIndex = 10;
            this.Rotation = 90;
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
            this.CollSensor = new CollisionSensor(this.virtualFunctionBus, this);
            this.LKA = new LaneKeepingAssistance(cameraSettings);
        }

        public RadarSensor RadarSensor { get;  }

        public CameraSensor CameraSensor { get; }

        public CollisionSensor CollSensor { get; }

        public LaneKeepingAssistance LKA { get; }

        public VirtualFunctionBus VirtualFunctionBus { get => this.virtualFunctionBus; }
        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "") => this.PropertyChanged?.Invoke(this, new(propertyName));

        public int Revolution
        {
            get
            {
                return this.revo;
            }

            set
            {
                this.revo = value;
                this.NotifyPropertyChanged(nameof(this.Revolution));
            }
        }

        public int Velocity
        {
            get
            {
                return this.velo;
            }

            set
            {
                this.velo = value;
                this.NotifyPropertyChanged(nameof(this.Velocity));
            }
        }
        public GearBox GearBox { get => this.gearBox; }

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