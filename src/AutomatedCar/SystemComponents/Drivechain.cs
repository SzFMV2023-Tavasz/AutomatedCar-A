namespace AutomatedCar.SystemComponents
{
    using AutomatedCar.Models;
    using AutomatedCar.SystemComponents.Packets;
    using Avalonia;
    using Avalonia.Layout;
    using System;
    using System.Diagnostics;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Numerics;
    using System.Reflection.Metadata;
    using System.Runtime.ConstrainedExecution;
    using DynamicData;

    public class Drivechain : SystemComponent
    {
        private DrivechainPacket drivechainPacket;
        private ICharacteristicsInterface characteristicsPacket;
        private ISteeringWheel SteeringWheelPacket;
        private double timeBetweenFrames = 1.0 / 120.0; // time between frames in seconds

        public Drivechain(VirtualFunctionBus virtualFunctionBus) : base(virtualFunctionBus)
        {
            this.drivechainPacket = new DrivechainPacket();
            virtualFunctionBus.drivechainPacket = this.drivechainPacket;
            this.characteristicsPacket = (ICharacteristicsInterface)virtualFunctionBus.CharacteristicsPacket;
            this.SteeringWheelPacket = (ISteeringWheel)virtualFunctionBus.SteeringWheelPacket;
        }

        public override void Process()
        {

            float speedKMH = this.characteristicsPacket.Speed; // given speed in km/h
            float speedMS = (float)(speedKMH / 3.6); // convert speed to m/s

            Vector2 direction = new Vector2(this.SteeringWheelPacket.DirectionVector.X, this.SteeringWheelPacket.DirectionVector.Y); // given direction vector

            float turningRadius = float.MaxValue; // set the turning radius to a large number initially

            // calculate angular velocity in radians per second
            if (direction != Vector2.Zero)
            {
                float angularVelocity;

                if ((direction.X == 0 && direction.Y == 1) || (direction.X == 0 && direction.Y == -1))
                {
                    angularVelocity = 0.0f;
                }

                else
                {
                    angularVelocity = (float)(speedMS / direction.Length());
                }

                if (angularVelocity != 0)
                {
                    turningRadius = (float)(speedMS / angularVelocity);
                }
            }

            float angle = (float)Math.Atan2(direction.Y, direction.X);

            // calculate motion vector
            Vector2 motion = new Vector2(
                (float)(speedMS * Math.Cos(angle - (Math.PI / 2.0))),
                (float)(speedMS * Math.Sin(angle - (Math.PI / 2.0)))) * (float)this.timeBetweenFrames * 30; // motion vector in meters

            // calculate new position
            Vector2 newPosition = new Vector2(World.Instance.ControlledCar.X, World.Instance.ControlledCar.Y) + (motion * 2); // * 2 for faster visual acceleration

            // calculate new rotation
            float newRotation = (float)World.Instance.ControlledCar.Rotation;

            if (turningRadius != float.MaxValue)
            {
                float angleChange = (float)(speedMS * this.timeBetweenFrames) / turningRadius;
                newRotation = (float)(angle - angleChange) * 180.0f / (float)Math.PI;
            }

            Vector2 diff = new(World.Instance.ControlledCar.X - newPosition.X, World.Instance.ControlledCar.Y - newPosition.Y);
            var a = diff.Length();
            this.drivechainPacket.vectorDifferentialLength = diff.Length();
            //Debug.WriteLine("DIFF LENGTH: " + a);

            // update car position and rotation
            World.Instance.ControlledCar.X = newPosition.X;
            World.Instance.ControlledCar.Y = newPosition.Y;
            World.Instance.ControlledCar.Rotation = newRotation;
            World.Instance.ControlledCar.Velocity = (int)speedKMH;

        }
    }
}
