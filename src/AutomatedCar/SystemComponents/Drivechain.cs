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
        private IPedalInterface pedalPacket;
        private ICharacteristicsInterface characteristicsPacket;
        private ISteeringWheel SteeringWheelPacket;
        private Vector2 motionVector;
        private Vector2D directionVector;
        int counter = 0;
        int pedpos = 1;

        public Drivechain(VirtualFunctionBus virtualFunctionBus) : base(virtualFunctionBus)
        {
            this.drivechainPacket = new DrivechainPacket();
            virtualFunctionBus.drivechainPacket = this.drivechainPacket;
            this.characteristicsPacket = (ICharacteristicsInterface)virtualFunctionBus.CharacteristicsPacket;
            this.pedalPacket = (IPedalInterface)virtualFunctionBus.GasPedalPacket;
            this.motionVector = new();
            this.SteeringWheelPacket = (ISteeringWheel)virtualFunctionBus.SteeringWheelPacket;
        }

        public override void Process()
        {

            //MUKODIK
            //double speedKMH = 100.0; // given speed in km/h
            //double speedMS = speedKMH / 3.6; // convert speed to m/s

            //Vector2 direction = new Vector2(10, 4); // given direction vector
            //float steeringAngle = 30f; // given steering angle in degrees

            //// calculate motion vector
            //double turningRadius = 2.5 / Math.Sin(steeringAngle * Math.PI / 180.0); // turning radius in meters
            //double tr = 90.0 / Math.Tan((steeringAngle * Math.PI) / 180.0);

            //double angularVelocity = speedMS / tr; // angular velocity in radians per second
            //double timeBetweenFrames = 1.0 / 60.0; // time between frames in seconds
            //double angle = Math.Atan2(direction.Y, direction.X);
            //Vector2 motion = new Vector2(
            //    (float)(speedMS * Math.Cos(angle + Math.PI / 2.0)),
            //    (float)(speedMS * Math.Sin(angle + Math.PI / 2.0))
            //) * (float)timeBetweenFrames; // motion vector in meters

            //// calculate new position
            //Vector2 newPosition = new Vector2(World.Instance.ControlledCar.X, World.Instance.ControlledCar.Y) + motion;

            //// calculate new rotation
            //float newRotation = (float)(angle + (angularVelocity * timeBetweenFrames)) * 180.0f / (float)Math.PI;

            //// update car position and rotation
            //World.Instance.ControlledCar.X = newPosition.X;
            //World.Instance.ControlledCar.Y = newPosition.Y;
            //World.Instance.ControlledCar.Rotation = newRotation + 180;

            //if (counter >= 300)
            //{
            //    pedpos = 0;
            //}

















            //EZ KELL
            //this.drivechainPacket.Speed = (int)this.characteristicsPacket.Speed;


            //float speedKMH = -50; // given speed in km/h
            //float speedMS = (float)(speedKMH / 3.6); // convert speed to m/s

            //Vector2 direction = new Vector2(this.SteeringWheelPacket.DirectionVector.X, this.SteeringWheelPacket.DirectionVector.Y); // given direction vector

            //// calculate motion vector
            //float turningRadius = float.MaxValue; // set the turning radius to a large number initially

            //if (direction != Vector2.Zero)
            //{
            //    float angularVelocity = (float)(speedMS / direction.Length());

            //    if (angularVelocity != 0)
            //    {
            //        turningRadius = (float)(speedMS / angularVelocity);
            //    }
            //}

            //// angular velocity in radians per second
            //double timeBetweenFrames = 1.0 / 120.0; // time between frames in seconds
            //float angle = (float)Math.Atan2(direction.Y, direction.X);

            //Vector2 motion = new Vector2((float)(speedMS * Math.Cos(angle + Math.PI / 2.0)), (float)(speedMS * Math.Sin(angle + Math.PI / 2.0))) * (float)timeBetweenFrames * 30; // motion vector in meters

            //if (pedpos == 0) // apply air drag only when gas pedal position is 0
            //{
            //    double dragCoefficient = 0.5; // drag coefficient for a car
            //    double frontalArea = 2.0; // frontal area of a car in m^2
            //    double airDensity = 1.2; // air density in kg/m^3
            //    double dragForce = 0.5 * dragCoefficient * frontalArea * airDensity * (speedMS * speedMS); // drag force in newtons
            //    double accelerationDueToDrag = dragForce / 1000.0; // acceleration due to drag in m/s^2
            //    double speedMSToApplyAirDrag = 10.0; // speed in m/s at which to apply the full drag force

            //    if (speedMS > speedMSToApplyAirDrag) // apply drag force only when speed is above a certain threshold
            //    {
            //        double airDragAcceleration = accelerationDueToDrag * ((speedMS - speedMSToApplyAirDrag) / speedMS);
            //        motion *= (float)(1.0 - (airDragAcceleration * timeBetweenFrames));
            //    }
            //}

            //// calculate new position
            //Vector2 newPosition = new Vector2(World.Instance.ControlledCar.X, World.Instance.ControlledCar.Y) + motion;

            //// calculate new rotation
            //float newRotation = (float)World.Instance.ControlledCar.Rotation;

            //if (turningRadius != double.MaxValue)
            //{
            //    double angleChange = (speedMS * timeBetweenFrames) / turningRadius;
            //    newRotation = (float)(angle + angleChange) * 180.0f / (float)Math.PI;
            //}

            //// update car position and rotation
            //World.Instance.ControlledCar.X = newPosition.X;
            //World.Instance.ControlledCar.Y = newPosition.Y;
            //World.Instance.ControlledCar.Rotation = newRotation;


            //Debug.WriteLine("rot: " + newRotation);
            //Debug.WriteLine("ang: " + angle);
            //Debug.WriteLine("dir: " + direction);


            //jol mukodik, jo khm-val!
            float speedKMH = 50; // given speed in km/h
            float speedMS = (float)(speedKMH / 3.6); // convert speed to m/s

            Vector2 direction = new Vector2(this.SteeringWheelPacket.DirectionVector.X, this.SteeringWheelPacket.DirectionVector.Y); // given direction vector

            // calculate motion vector
            float turningRadius = float.MaxValue; // set the turning radius to a large number initially

            if (direction != Vector2.Zero)
            {
                float angularVelocity = (float)(speedMS / direction.Length());

                if (angularVelocity != 0)
                {
                    turningRadius = (float)(speedMS / angularVelocity);
                }
            }

            // angular velocity in radians per second
            double timeBetweenFrames = 1.0 / 120.0; // time between frames in seconds


            float angle = (float)Math.Atan2(direction.Y, direction.X);

            Vector2 motion = new Vector2(
                (float)(speedMS * Math.Cos(angle - Math.PI / 2.0)),
                (float)(speedMS * Math.Sin(angle - Math.PI / 2.0))
            ) * (float)timeBetweenFrames * 30; // motion vector in meters

            if (pedpos == 0) // apply air drag only when gas pedal position is 0
            {
                double dragCoefficient = 0.5; // drag coefficient for a car
                double frontalArea = 2.0; // frontal area of a car in m^2
                double airDensity = 1.2; // air density in kg/m^3
                double dragForce = 0.5 * dragCoefficient * frontalArea * airDensity * (speedMS * speedMS); // drag force in newtons
                double accelerationDueToDrag = dragForce / 1000.0; // acceleration due to drag in m/s^2
                double speedMSToApplyAirDrag = 10.0; // speed in m/s at which to apply the full drag force

                if (speedMS > speedMSToApplyAirDrag) // apply drag force only when speed is above a certain threshold
                {
                    double airDragAcceleration = accelerationDueToDrag * ((speedMS - speedMSToApplyAirDrag) / speedMS);
                    motion *= (float)(1.0 - (airDragAcceleration * timeBetweenFrames));
                }
            }

            // calculate new position
            Vector2 newPosition = new Vector2(World.Instance.ControlledCar.X, World.Instance.ControlledCar.Y) + motion;

            // calculate new rotation
            float newRotation = (float)World.Instance.ControlledCar.Rotation;

            if (turningRadius != float.MaxValue)
            {
                double angleChange = (speedMS * timeBetweenFrames) / turningRadius;
                newRotation = (float)(angle - angleChange) * 180.0f / (float)Math.PI;
            }

            // update car position and rotation
            World.Instance.ControlledCar.X = newPosition.X;
            World.Instance.ControlledCar.Y = newPosition.Y;
            World.Instance.ControlledCar.Rotation = newRotation;

            Debug.WriteLine("rot: " + newRotation);
            Debug.WriteLine("ang: " + angle);
            Debug.WriteLine("dir: " + direction);


        }

        public static double AngleBetween(Vector2 v1, Vector2 v2)
        {
            double angle = Math.Atan2(v2.Y - v1.Y, v2.X - v1.X);
            return angle;
        }

    }
}
