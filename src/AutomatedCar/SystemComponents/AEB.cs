namespace AutomatedCar.SystemComponents
{
    using AutomatedCar.Models;
    using AutomatedCar.SystemComponents.Packets;
    using Avalonia.Threading;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Numerics;
    using System.Reflection;
    using System.Text;
    using System.Threading.Tasks;

    public class AEB : SystemComponent
    {
        private static double initialOpacity_brake;
        private static double flashOpacity_brake;
        private static bool isFlashing_brake;
        private static double initialOpacity_yellow;
        private static double flashOpacity_yellow;
        private static bool isFlashing_yellow;
        private static DispatcherTimer timer;
        private static DispatcherTimer timer2;

        private AEBPacket aebPacket;
        private IReadOnlyPacket<DetectedObjectInfo> radarPacket;
        private IReadOnlyPacket<DetectedObjectInfo> onWayToCollidePacket;
        private double sensorOffset = 85.0;

        public AEB(VirtualFunctionBus virtualFunctionBus) : base(virtualFunctionBus)
        {
            this.aebPacket = new AEBPacket();
            virtualFunctionBus.AEBPacket = this.aebPacket;
            this.radarPacket = (IReadOnlyPacket<DetectedObjectInfo>)virtualFunctionBus.RadarPacket;
            this.onWayToCollidePacket = (IReadOnlyPacket<DetectedObjectInfo>)virtualFunctionBus.OnWayToCollidePacket;

            //switch (World.Instance.ControlledCar.Filename)
            //{
            //    case "car_1_white.png": this.sensorOffset = 85.0; break;
            //    default: break;
            //}
        }

        public override void Process()
        {
            // 9ms2 dec. = 32.4 km/h/s = 0.54 km/process
            // 70 kmh = 9,722146 (129.63 * 0.54) pixel differential

            //14.52380952 * meters = pixels
            //+85px to get car front -- 390 (305 + 85) with 70kmh

            this.DetectStaticObjectCollision();

            if (World.Instance.ControlledCar.Velocity > 70)
            {
                if (timer == null)
                {
                    this.ApplyFlashEffectYellow(TimeSpan.FromSeconds(0.2));
                }
            }

            else
            {
                this.RemoveFlashEffectYellow();
            }
        }

        private void DetectStaticObjectCollision()
        {
            double speedMs = World.Instance.ControlledCar.Velocity * (1000.0 / 3600.0);

            double distance = (Math.Pow(speedMs, 2) / (2.0 * 9.0)) * 14.52380952;
            double activationalDistance = (double)(distance + this.sensorOffset);

            //Debug.WriteLine("ACT: " + activationalDistance);

            bool insideActivationalDistance = this.radarPacket.WorldObjectsDetected.Any(x => x.Distance < activationalDistance * 1.4) ||
                                              this.onWayToCollidePacket.WorldObjectsDetected.Any(x => x.Distance < activationalDistance * 1.4);

            if (insideActivationalDistance && speedMs > 0)
            {
                if (this.aebPacket.RedWarning != 1)
                {
                    if (timer2 == null)
                    {
                        this.ApplyFlashEffectBreak(TimeSpan.FromSeconds(0.2));
                    }
                }

                if (this.radarPacket.WorldObjectsDetected.Any(x => x.Distance < activationalDistance) || this.onWayToCollidePacket.WorldObjectsDetected.Any(x => x.Distance < activationalDistance))
                {
                    this.RemoveFlashEffectBreak();
                    this.aebPacket.RedWarning = 1;
                    this.aebPacket.AEBIsActive = true;
                }
            }

            else
            {
                this.RemoveFlashEffectBreak();
            }

            if (World.Instance.ControlledCar.Velocity == 0)
            {
                this.aebPacket.AEBIsActive = false;
                this.aebPacket.RedWarning = 0;
            }
        }

        private void ApplyFlashEffectYellow(TimeSpan flashInterval)
        {
            initialOpacity_yellow = this.aebPacket.YellowWarning;
            flashOpacity_yellow = initialOpacity_yellow == 1 ? 0 : 1;
            isFlashing_yellow = true;

            timer = new DispatcherTimer(DispatcherPriority.Render);
            timer.Interval = flashInterval;
            timer.Tick += (sender, e) =>
            {
                if (isFlashing_yellow)
                {
                    this.aebPacket.YellowWarning = this.aebPacket.YellowWarning == initialOpacity_yellow ? flashOpacity_yellow : initialOpacity_yellow;
                }
            };

            timer.Start();
        }

        private void ApplyFlashEffectBreak(TimeSpan flashInterval)
        {
            initialOpacity_brake = this.aebPacket.BreakWarning;
            flashOpacity_brake = initialOpacity_brake == 1 ? 0 : 1;
            isFlashing_brake = true;

            timer2 = new DispatcherTimer(DispatcherPriority.Render);
            timer2.Interval = flashInterval;
            timer2.Tick += (sender, e) =>
            {
                if (isFlashing_brake)
                {
                    this.aebPacket.BreakWarning = this.aebPacket.BreakWarning == initialOpacity_brake ? flashOpacity_brake : initialOpacity_brake;
                }
            };

            timer2.Start();
        }


        private void RemoveFlashEffectYellow()
        {
            if (timer != null)
            {
                timer.Stop();
                timer = null;
            }

            this.aebPacket.YellowWarning = 0;
            isFlashing_yellow = false;
        }

        private void RemoveFlashEffectBreak()
        {
            if (timer2 != null)
            {
                timer2.Stop();
                timer2 = null;
            }

            this.aebPacket.BreakWarning = 0;
            isFlashing_brake = false;
        }
    }
}
