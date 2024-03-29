using System.Collections.ObjectModel;
using AutomatedCar.Models;
using System.Linq;

using ReactiveUI;

namespace AutomatedCar.ViewModels
{
    using Avalonia.Controls;
    using Models;
    using System;
    using System.Diagnostics;
    using Visualization;

    public class CourseDisplayViewModel : ViewModelBase
    {
        public ObservableCollection<WorldObjectViewModel> WorldObjects { get; } = new ObservableCollection<WorldObjectViewModel>();

        private Avalonia.Vector offset;

        public CourseDisplayViewModel(World world)
        {
            this.WorldObjects = new ObservableCollection<WorldObjectViewModel>(world.WorldObjects.Select(wo => new WorldObjectViewModel(wo)));
            this.Width = world.Width;
            this.Height = world.Height;
        }

        public int Width { get; set; }

        public int Height { get; set; }

        public Avalonia.Vector Offset
        {
            get => this.offset;
            set => this.RaiseAndSetIfChanged(ref this.offset, value);
        }

        private DebugStatus debugStatus = new DebugStatus();

        public DebugStatus DebugStatus
        {
            get => this.debugStatus;
            set => this.RaiseAndSetIfChanged(ref this.debugStatus, value);
        }

        public void KeyUp()
        {
            World.Instance.ControlledCar.GasPedal.isPedalPressed = true;
            //World.Instance.ControlledCar.Y -= 5;
        }

        public void KeyDown()
        {
            World.Instance.ControlledCar.BrakePedal.isPedalPressed = true;
            World.Instance.ControlledCar.CruiseControl.SetIsActiveFalse();
            //World.Instance.ControlledCar.Y += 5;
        }

        public void KeyLeft()
        {
            //World.Instance.ControlledCar.X -= 5;
            World.Instance.ControlledCar.steeringWheel.TurnWheel(Helpers.SteeringWheelDirectionEnum.TurnRight, true);
        }

        public void KeyRight()
        {
            //World.Instance.ControlledCar.X += 5;
            World.Instance.ControlledCar.steeringWheel.TurnWheel(Helpers.SteeringWheelDirectionEnum.TurnLeft, true);
        }

        public void PageUp()
        {
            //World.Instance.ControlledCar.Rotation += 5;
        }

        public void PageDown()
        {
            // World.Instance.ControlledCar.Rotation -= 5;
        }

        public void ToggleDebug()
        {
            this.debugStatus.Enabled = !this.debugStatus.Enabled;
        }

        public void ToggleCamera()
        {
            this.DebugStatus.Camera = !this.DebugStatus.Camera;
        }

        public void ShiftUp()
        {
            World.Instance.ControlledCar.GearBox.OuterGearShiftUp();
        }

        public void ShiftDown()
        {
            World.Instance.ControlledCar.GearBox.OuterGearShiftDown();
        }

        public void ToggleRadar()
        {
            // World.Instance.DebugStatus.Radar = !World.Instance.DebugStatus.Radar;
        }

        public void ToggleUltrasonic()
        {
            //World.Instance.DebugStatus.Ultrasonic = !World.Instance.DebugStatus.Ultrasonic;
        }

        public void ToggleRotation()
        {
            //World.Instance.DebugStatus.Rotate = !World.Instance.DebugStatus.Rotate;
        }

        public void FocusCar(ScrollViewer scrollViewer)
        {
            var offsetX = World.Instance.ControlledCar.X - (scrollViewer.Viewport.Width / 2);
            var offsetY = World.Instance.ControlledCar.Y - (scrollViewer.Viewport.Height / 2);
            this.Offset = new Avalonia.Vector(offsetX, offsetY);
        }

        internal void ToggleAdaptiveTempomat()
        {
            World.Instance.ControlledCar.CruiseControl.ToggleCruiseControl();
        }

        internal void DecreaseAccTargetSpeed()
        {
            World.Instance.ControlledCar.CruiseControl.DecreaseTargetSpeed();
        }

        internal void IncreaseAccTargetSpeed()
        {
            World.Instance.ControlledCar.CruiseControl.IncreaseTargetSpeed();
        }

        internal void ChangeAccTargetDistance()
        {
            World.Instance.ControlledCar.CruiseControl.ChangeTargetDistance();
        }
    }
}