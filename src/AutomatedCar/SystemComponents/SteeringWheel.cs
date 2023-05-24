﻿namespace AutomatedCar.SystemComponents
{
    using AutomatedCar.Helpers;
    using AutomatedCar.Models;
    using AutomatedCar.SystemComponents.Packets;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Threading.Tasks;

    public class SteeringWheel : SystemComponent
    {
        public SteeringWheelPacket steeringWheelPacket = new SteeringWheelPacket();
        private SteeringWheelDirectionEnum steeringWheelDirection = SteeringWheelDirectionEnum.Hold;

        private Vector2D baseVector = new Vector2D(0, 1);
        private Vector2D maxLeftVector;
        private Vector2D maxRightVector;
        private float maxAngle = 89.9f;
        private float turnAngle = 2f;
        private int ownDefaultTickCounter = 1;
        private int ownCurrentTick = 0;

        public SteeringWheel(VirtualFunctionBus virtualFunctionBus) : base(virtualFunctionBus)
        {
            virtualFunctionBus.SteeringWheelPacket = this.steeringWheelPacket;
            this.steeringWheelPacket.DirectionVector = this.baseVector;
            this.maxLeftVector = this.Rotate(this.baseVector, this.maxAngle);
            this.maxRightVector = this.Rotate(this.baseVector, this.maxAngle * -1);
            this.ownCurrentTick = this.ownDefaultTickCounter;
        }

        public void TurnWheel(SteeringWheelDirectionEnum direction, bool driverSteering = false)
        {
            if(driverSteering)
            {
                this.TurnOffLKA();
            }
            this.steeringWheelDirection = direction;
        }

        public override void Process()
        {
            if (!this.OwnTick())
            {
                return;
            }

            Vector2D currentVector = this.steeringWheelPacket.DirectionVector;

            double vectorMagnitudeDiff = Math.Round((currentVector - baseVector).Magnitude, 10);
            if (this.steeringWheelDirection == SteeringWheelDirectionEnum.Hold && vectorMagnitudeDiff == 0)
            {
                return;
            }
            else if(steeringWheelDirection == SteeringWheelDirectionEnum.Hold)
            {
                //this.SetServoDirection();
            }
            
            if(this.steeringWheelDirection == SteeringWheelDirectionEnum.TurnLeft)
            {
                this.steeringWheelPacket.DirectionVector = this.Rotate(currentVector, this.turnAngle);
            }

            if(this.steeringWheelDirection == SteeringWheelDirectionEnum.TurnRight)
            {
                this.steeringWheelPacket.DirectionVector = this.Rotate(currentVector, this.turnAngle * -1);
            }

            this.ResetSettings();
        }

        private Vector2D Rotate(Vector2D vector, float angle)
        {
            Vector2D newVector = new Vector2D(vector.X, vector.Y);
            newVector.Rotate(angle);

            if (maxLeftVector != null && newVector.X < maxLeftVector.X)
            {
                return maxLeftVector;
            }

            if(maxRightVector != null && newVector.X > maxRightVector.X)
            {
                return maxRightVector;
            }

            return newVector;
        }

        private void SetServoDirection()
        {
            if (this.steeringWheelPacket.DirectionVector.X < baseVector.X)
            {
                this.steeringWheelDirection = SteeringWheelDirectionEnum.TurnRight;
            }

            if (this.steeringWheelPacket.DirectionVector.X > baseVector.X)
            {
                this.steeringWheelDirection = SteeringWheelDirectionEnum.TurnLeft;
            }
        }

        private void ResetSettings()
        {
            this.steeringWheelDirection = SteeringWheelDirectionEnum.Hold;
        }

        private bool OwnTick()
        {
            this.ownCurrentTick--;

            if(this.ownCurrentTick <= 0)
            {
                this.ownCurrentTick = this.ownDefaultTickCounter;
                return true;
            }

            return false;
        }

        private void TurnOffLKA()
        {
            this.virtualFunctionBus.LKANotifierPacket.Intervention = true;
        }
    }
}
