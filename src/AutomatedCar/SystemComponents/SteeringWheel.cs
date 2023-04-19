namespace AutomatedCar.SystemComponents
{
    using AutomatedCar.Helpers;
    using AutomatedCar.Models;
    using AutomatedCar.SystemComponents.Packets;
    using System;
    using System.Collections.Generic;
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
        private float maxAngle = 60.0f;
        public SteeringWheel(VirtualFunctionBus virtualFunctionBus) : base(virtualFunctionBus)
        {
            virtualFunctionBus.SteeringWheelPacket = this.steeringWheelPacket;
            this.steeringWheelPacket.DirectionVector = this.baseVector;
            this.maxLeftVector = this.Rotate(this.baseVector, this.maxAngle * -1);
            this.maxRightVector = this.Rotate(this.baseVector, this.maxAngle);
        }

        public void TurnWheel(SteeringWheelDirectionEnum direction)
        {
            this.steeringWheelDirection = direction;
        }

        public override void Process()
        {
            Vector2D currentVector = this.steeringWheelPacket.DirectionVector;
            
            if(this.steeringWheelDirection == SteeringWheelDirectionEnum.Hold && currentVector == baseVector)
            {
                return;
            }
            else if(steeringWheelDirection == SteeringWheelDirectionEnum.Hold)
            {
                this.SetServoDirection();
            }
            
            if(this.steeringWheelDirection == SteeringWheelDirectionEnum.TurnLeft)
            {
                this.steeringWheelPacket.DirectionVector = this.Rotate(currentVector, -0.1f);
            }

            if(this.steeringWheelDirection == SteeringWheelDirectionEnum.TurnRight)
            {
                this.steeringWheelPacket.DirectionVector = this.Rotate(currentVector, 0.1f);
            }

            this.ResetSettings();

            System.Diagnostics.Debug.WriteLine("X: {0}, Y:{0}", this.steeringWheelPacket.DirectionVector.X, this.steeringWheelPacket.DirectionVector.Y);
        }

        private Vector2D Rotate(Vector2D vector, float angle)
        {
            double theta = this.DegreeToRadian(angle);
            double cos = Math.Cos(theta);
            double sin = Math.Sin(theta);

            float newX = (float)(vector.X * cos + vector.Y * sin);
            float newY = (float)(vector.X * sin + vector.Y * cos);

            Vector2D newVector = new Vector2D(newX, newY);

            if(maxLeftVector != null && newVector.Magnitude < maxLeftVector.Magnitude)
            {
                return maxLeftVector;
            }

            if(maxRightVector != null && newVector.Magnitude > maxRightVector.Magnitude)
            {
                return maxRightVector;
            }

            return newVector;
        }

        private double DegreeToRadian(double angle)
        {
            return (Math.PI / 1) * angle;
        }

        private void SetServoDirection()
        {
            if (this.steeringWheelPacket.DirectionVector.Magnitude < maxLeftVector.Magnitude)
            {
                this.steeringWheelDirection = SteeringWheelDirectionEnum.TurnRight;
            }

            if (this.steeringWheelPacket.DirectionVector.Magnitude > maxRightVector.Magnitude)
            {
                this.steeringWheelDirection = SteeringWheelDirectionEnum.TurnLeft;
            }
        }

        private void ResetSettings()
        {
            this.steeringWheelDirection = SteeringWheelDirectionEnum.Hold;
        }
    }
}
