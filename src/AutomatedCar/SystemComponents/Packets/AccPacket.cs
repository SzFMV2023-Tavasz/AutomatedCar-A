namespace AutomatedCar.SystemComponents.Packets
{
    using ReactiveUI;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    internal class AccPacket : ReactiveObject, IAccInterface
    {
        bool isActive;
        double selectedTargetDistance;
        int selectedSpeed;
        public bool IsActive { get => this.isActive; set => this.RaiseAndSetIfChanged(ref this.isActive, value); }

        public double SelectedTargetDistance { get => this.selectedTargetDistance; set => this.RaiseAndSetIfChanged(ref this.selectedTargetDistance, value); }

        public int SelectedSpeed { get => this.selectedSpeed; set => this.RaiseAndSetIfChanged(ref this.selectedSpeed, value); }

    }
}
