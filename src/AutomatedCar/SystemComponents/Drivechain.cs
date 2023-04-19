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

        //float steeringAngle = -20f; // given steering angle in degrees
        Vector2 direction = new Vector2(4, -5); // given direction vector


        public Drivechain(VirtualFunctionBus virtualFunctionBus) : base(virtualFunctionBus)
        {
            this.drivechainPacket = new DrivechainPacket();
            virtualFunctionBus.drivechainPacket = this.drivechainPacket;
            this.characteristicsPacket = (ICharacteristicsInterface)virtualFunctionBus.CharacteristicsPacket;
            //this.pedalPacket = (IPedalInterface)virtualFunctionBus.PedalPacket;
            this.motionVector = new();
            //this.directionVector = SteeringWheelPacket.DirectionVector;
            //this.carHeading = carHeading = (float)Math.Atan2(frontWheelY - backWheelY, frontWheelX - backWheelX);
        }

        public override void Process()
        {
            counter++;
            //this.directionVector = SteeringWheelPacket.DirectionVector;
            // this.carLocation = new(World.Instance.ControlledCar.X, World.Instance.ControlledCar.Y);

            //// Define the car's initial position, speed, and direction vector
            //Vector2 carPosition = new Vector2(World.Instance.ControlledCar.X, World.Instance.ControlledCar.Y); // Change values as per requirement
            //float carSpeed = 20; // Change value as per requirement

            //Vector2 motionVector = new Vector2(1, 0); // Change values as per requirement
            //this.radorientation = (float)(this.steerAngle * Math.PI / 180);
            ////float newOrientation = radorientation + DirectionVector;

            ////motionVector = new Vector2((float)Math.Cos(radorientation), (float)Math.Sin(radorientation)) * (float)(carSpeed / 3.6f);

            //Vector2 directionVector = new(); // Change values as per requirement

            //// Calculate the velocity vector by multiplying the motion vector with the speed
            //Vector2 velocityVector = Vector2.Normalize(motionVector) * (carSpeed / 3.6f); // Convert speed from km/h to m/s

            //// Rotate the motion vector and direction vector based on a desired angle (in radians)
            //double angle = steerAngle * Math.PI / 180; // Change angle as per requirement
            //Matrix3x2 rotationMatrix = Matrix3x2.CreateRotation((float)angle);
            //motionVector = Vector2.Transform(motionVector, rotationMatrix);
            //directionVector = Vector2.Transform(directionVector, rotationMatrix);

            //float a = (float)Math.Atan2(motionVector.Y, motionVector.X);

            //// Rotate the car's sprite to face the angle
            //World.Instance.ControlledCar.Rotation = steerAngle;

            //World.Instance.ControlledCar.X += velocityVector.X * motionVector.X + velocityVector.Y * directionVector.X;
            //World.Instance.ControlledCar.Y -= velocityVector.X * motionVector.Y + velocityVector.Y * directionVector.Y;


            //EZ MUKODIK!!!!!!!!!!!!!
            //double speedKMH = 50; // given speed in km/h
            //float speedMS = (float)(speedKMH / 3.6); // convert speed to m/s

            //Vector2 direction = new Vector2(10, 3); // given direction vector
            //float steeringAngle = 30f; // given steering angle in degrees

            //// calculate motion vector
            //float turningRadius = (float)(2.5 / Math.Sin(steeringAngle * Math.PI / 180.0)); // turning radius in meters
            //float angularVelocity = speedMS / turningRadius; // angular velocity in radians per second
            //double timeBetweenFrames = 1.0 / 60.0; // time between frames in seconds
            //Vector2 motion = new Vector2(
            //    (float)(speedMS * Math.Cos(AngleBetween(new Vector2(0, 0), direction) + Math.PI / 2.0)),
            //    (float)(speedMS * Math.Sin(AngleBetween(new Vector2(0, 0), direction) + Math.PI / 2.0))
            //) * (float)timeBetweenFrames; // motion vector in meters

            //// calculate new position
            //Vector2 newPosition = new Vector2(World.Instance.ControlledCar.X, World.Instance.ControlledCar.Y) + motion;

            //// calculate new rotation
            //float newRotation = (float)(AngleBetween(new Vector2(0, 0), direction) + (angularVelocity * timeBetweenFrames)) * 180.0f / (float)Math.PI;

            //// update car position and rotation
            //World.Instance.ControlledCar.X = newPosition.X;
            //World.Instance.ControlledCar.Y = newPosition.Y;
            //World.Instance.ControlledCar.Rotation = newRotation;






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




            //Air drag only calculated with pedalpos = 0

            //double speedKMH = 100.0; // given speed in km/h
            //double speedMS = speedKMH / 3.6; // convert speed to m/s

            //Vector2 direction = new Vector2(2, -3); // given direction vector
            ////Vector2 direction = new Vector2(this.SteeringWheelPacket.DirectionVector.X, this.SteeringWheelPacket.DirectionVector.Y); // given direction vector
            ////steeringAngle += 1f; ; // given steering angle in degrees

            //// calculate motion vector
            //double turningRadius = 2.5 / Math.Sin(steeringAngle * Math.PI / 180.0);  // turning radius in meters

            //double angularVelocity = speedMS / (turningRadius + 0.5);
            //if (steeringAngle < 0)
            //{
            //    angularVelocity = -angularVelocity;
            //}

            //// angular velocity in radians per second
            //double timeBetweenFrames = 1.0 / 60.0; // time between frames in seconds
            //double angle = Math.Atan2(direction.Y, direction.X);

            //Vector2 motion = new Vector2((float)(speedMS * Math.Cos(angle + Math.PI / 2.0)), (float)(speedMS * Math.Sin(angle + Math.PI / 2.0))) * (float)timeBetweenFrames; // motion vector in meters

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
            //float newRotation = (float)(angle + (angularVelocity * timeBetweenFrames)) * 180.0f / (float)Math.PI;

            //// update car position and rotation
            //World.Instance.ControlledCar.X = newPosition.X;
            //World.Instance.ControlledCar.Y = newPosition.Y;
            //World.Instance.ControlledCar.Rotation = newRotation + 180;














            double speedKMH = 90; // given speed in km/h
            double speedMS = speedKMH / 3.6; // convert speed to m/s

            //Vector2 direction = new Vector2(2, -3); // given direction vector

            // calculate motion vector
            double turningRadius = double.MaxValue; // set the turning radius to a large number initially

            if (direction != Vector2.Zero)
            {
                double angularVelocity = speedMS / direction.Length();

                if (angularVelocity != 0)
                {
                    turningRadius = speedMS / angularVelocity;
                }
            }

            // angular velocity in radians per second
            double timeBetweenFrames = 1.0 / 30.0; // time between frames in seconds
            double angle = Math.Atan2(direction.Y, direction.X);

            Vector2 motion = new Vector2((float)(speedMS * Math.Cos(angle + Math.PI / 2.0)), (float)(speedMS * Math.Sin(angle + Math.PI / 2.0))) * (float)timeBetweenFrames * 3; // motion vector in meters

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

            if (turningRadius != double.MaxValue)
            {
                double angleChange = (speedMS * timeBetweenFrames) / turningRadius;
                newRotation = (float)(angle + angleChange) * 180.0f / (float)Math.PI;
            }

            // update car position and rotation
            World.Instance.ControlledCar.X = newPosition.X;
            World.Instance.ControlledCar.Y = newPosition.Y;
            World.Instance.ControlledCar.Rotation = newRotation + 180;












            //double maxSteeringAngle = 60.0; // maximum steering angle in degrees
            //double maxSteeringRadians = Math.PI / 180.0 * maxSteeringAngle; // maximum steering angle in radians

            //double speedKMH = 50.0; // given speed in km/h
            //double speedMS = speedKMH / 3.6; // convert speed to m/s

            ////Vector2 direction = new Vector2(2, -3); // given direction vector

            //// calculate motion vector
            //double turningRadius = double.MaxValue; // set the turning radius to a large number initially

            //if (direction != Vector2.Zero)
            //{
            //    double angularVelocity = speedMS / direction.Length() + 1;

            //    if (angularVelocity != 0)
            //    {
            //        double maxTurningRadius = Math.Tan(maxSteeringRadians) * direction.Length() / angularVelocity;
            //        turningRadius = Math.Min(turningRadius, maxTurningRadius);
            //    }
            //}

            //// angular velocity in radians per second
            //double timeBetweenFrames = 1.0 / 10.0; // time between frames in seconds
            //double angle = Math.Atan2(direction.Y, direction.X);

            //Vector2 motion = new Vector2((float)(speedMS * Math.Cos(angle + Math.PI / 2.0)), (float)(speedMS * Math.Sin(angle + Math.PI / 2.0))) * (float)timeBetweenFrames; // motion vector in meters

            //if (pedpos == 0) // apply air drag only when gas pedal position is 0
            //{
            //    double dragCoefficient = 0.5; // drag coefficient for a car
            //    double frontalArea = 1.0; // frontal area of a car in m^2
            //    double airDensity = 1.2; // air density in kg/m^3
            //    double dragForce = 0.2 * dragCoefficient * frontalArea * airDensity * (speedMS * speedMS); // drag force in newtons
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
            //World.Instance.ControlledCar.Rotation = newRotation + 180;






            //double maxSteeringAngle = 60.0; // maximum steering angle in degrees
            //double maxSteeringRadians = Math.PI / 180.0 * maxSteeringAngle; // maximum steering angle in radians

            //double speedKMH = 80.0; // given speed in km/h
            //double speedMS = speedKMH / 3.6; // convert speed to m/s

            ////Vector2 direction = new Vector2(2, -3); // given direction vector

            //// calculate motion vector
            //double turningRadius = double.MaxValue; // set the turning radius to a large number initially

            //if (direction != Vector2.Zero)
            //{
            //    double angularVelocity = speedMS / direction.Length() + 1;

            //    if (angularVelocity != 0)
            //    {
            //        double maxTurningRadius = Math.Tan(maxSteeringRadians) * direction.Length() / angularVelocity;
            //        turningRadius = Math.Min(turningRadius, maxTurningRadius);
            //    }
            //}

            //// angular velocity in radians per second
            //double timeBetweenFrames = 1.0 / 30.0; // time between frames in seconds
            //double angle = Math.Atan2(direction.Y, direction.X);

            //Vector2 motion = new Vector2((float)(speedMS * Math.Cos(angle + Math.PI / 2.0)), (float)(speedMS * Math.Sin(angle + Math.PI / 2.0))) * (float)(1.0 / 9.0); // motion vector in meters

            //if (pedpos == 0) // apply air drag only when gas pedal position is 0
            //{
            //    double dragCoefficient = 0.5; // drag coefficient for a car
            //    double frontalArea = 1.0; // frontal area of a car in m^2
            //    double airDensity = 1.2; // air density in kg/m^3
            //    double dragForce = 0.2 * dragCoefficient * frontalArea * airDensity * (speedMS * speedMS); // drag force in newtons
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
            //World.Instance.ControlledCar.Rotation = newRotation + 180;


        }

        public static double AngleBetween(Vector2 v1, Vector2 v2)
        {
            double angle = Math.Atan2(v2.Y - v1.Y, v2.X - v1.X);
            return angle;
        }

    }
}
