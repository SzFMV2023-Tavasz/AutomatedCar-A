﻿namespace AutomatedCar.SystemComponents
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
        private Vector2 motionVector;
        private Vector2D directionVector;
        double timeBetweenFrames = 1.0 / 120.0; // time between frames in seconds

        public Drivechain(VirtualFunctionBus virtualFunctionBus) : base(virtualFunctionBus)
        {
            this.drivechainPacket = new DrivechainPacket();
            virtualFunctionBus.drivechainPacket = this.drivechainPacket;
            this.characteristicsPacket = (ICharacteristicsInterface)virtualFunctionBus.CharacteristicsPacket;
            this.motionVector = new();
            this.SteeringWheelPacket = (ISteeringWheel)virtualFunctionBus.SteeringWheelPacket;
        }

        public override void Process()
        {

            float speedKMH = this.characteristicsPacket.Speed; // given speed in km/h
            float speedMS = (float)(speedKMH / 3.6); // convert speed to m/s

            Vector2 direction = new Vector2(this.SteeringWheelPacket.DirectionVector.X, this.SteeringWheelPacket.DirectionVector.Y); // given direction vector

            // calculate motion vector
            float turningRadius = float.MaxValue; // set the turning radius to a large number initially

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

            // angular velocity in radians per second


            float angle = (float)Math.Atan2(direction.Y, direction.X);
            Vector2 motion = new Vector2(
                (float)(speedMS * Math.Cos(angle - Math.PI / 2.0)),
                (float)(speedMS * Math.Sin(angle - Math.PI / 2.0))) * (float)timeBetweenFrames * 30; // motion vector in meters

            // calculate new position
            Vector2 newPosition = new Vector2(World.Instance.ControlledCar.X, World.Instance.ControlledCar.Y) + motion;

            // calculate new rotation
            float newRotation = (float)World.Instance.ControlledCar.Rotation;

            if (turningRadius != float.MaxValue)
            {
                float angleChange = (float)(speedMS * timeBetweenFrames) / turningRadius;
                newRotation = (float)(angle - angleChange) * 180.0f / (float)Math.PI;
            }

            // update car position and rotation
            World.Instance.ControlledCar.X = newPosition.X;
            World.Instance.ControlledCar.Y = newPosition.Y;
            World.Instance.ControlledCar.Rotation = newRotation;
            World.Instance.ControlledCar.Velocity = (int)speedKMH;
        }

        public static double AngleBetween(Vector2 v1, Vector2 v2)
        {
            double angle = Math.Atan2(v2.Y - v1.Y, v2.X - v1.X);
            return angle;
        }

    }
}
