namespace AutomatedCar.Models
{
    using Avalonia.Media;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using SystemComponents;

    public class AutomatedCar : Car, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private VirtualFunctionBus virtualFunctionBus;
        private GearBox gearBox;
        private Drivechain drivechain;

        private int velo;
        private int revo;

        public AutomatedCar(float x, float y, string filename)
            : base(x, y, filename)
        {
            this.virtualFunctionBus = new VirtualFunctionBus();
            this.gearBox = new GearBox(this.virtualFunctionBus);
            this.drivechain = new Drivechain(this.virtualFunctionBus);
            this.ZIndex = 10;
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