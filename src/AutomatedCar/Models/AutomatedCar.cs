using AutomatedCar.SystemComponents;

namespace AutomatedCar.Models
{
    using Avalonia.Media;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
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
            this.characteristics = new Characteristics(this.virtualFunctionBus);
            this.drivechain = new Drivechain(this.virtualFunctionBus);
            this.ZIndex = 10;
            this.Rotation = 90;
        }

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