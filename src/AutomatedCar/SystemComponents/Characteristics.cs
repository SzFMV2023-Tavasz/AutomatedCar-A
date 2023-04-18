namespace AutomatedCar.SystemComponents
{

    using AutomatedCar.SystemComponents.Packets;
    using System;
    public class Characteristics : SystemComponent
    {
        private ICharacteristicsInterface characteristicsPacket;
        private IGearboxInterface gearbox;
        private IBrakePedalInterface brakePedal;
        private IGasPedalInterface gasPedal;

        private const double WheelCircumference = 24 * Math.PI; // 24-inch wheel size converted to circumference
        private const double BrakeDeceleration = 9; // Deceleration rate 9m/s^2

        public Characteristics(VirtualFunctionBus virtualFunctionBus, IGearboxInterface gearbox, IBrakePedalInterface brakePedal, IGasPedalInterface gasPedal)
            : base(virtualFunctionBus)
        {
            this.characteristicsPacket = new CharacteristicsPacket();
            this.gearbox = gearbox;
            this.brakePedal = brakePedal;
            this.gasPedal = gasPedal;
            virtualFunctionBus.CharacteristicsPacket = this.characteristicsPacket;
        }

        public override void Process(double timeDelta)
        {
            double gearFactor = GetGearFactor(gearbox.InnerGear);
            double acceleration = CalculateAcceleration(gasPedal.GasPedalState, gearFactor, timeDelta);
            double deceleration = CalculateDeceleration(brakePedal.BrakePedalState, timeDelta);

            double netAcceleration = acceleration - deceleration;
            characteristicsPacket.Speed = CalculateSpeed(characteristicsPacket.Speed, netAcceleration, timeDelta);
            characteristicsPacket.RPM = CalculateRPM(characteristicsPacket.Speed);
        }

        private double GetGearFactor(int innerGear)
        {
            switch (innerGear)
            {
                case 1:
                    return 1.0;
                case 2:
                    return 0.8;
                case 3:
                    return 0.6;
                case 4:
                    return 0.4;
                case 5:
                    return 0.2;
                default:
                    return 1.0;
            }
        }

        private double CalculateAcceleration(int gasPedalState, double gearFactor, double timeDelta)
        {
            return Math.Sqrt(gasPedalState * gearFactor) * timeDelta;
        }

        private double CalculateDeceleration(int brakePedalState, double timeDelta)
        {
            if (brakePedalState > 0)
            {
                return BrakeDeceleration * (brakePedalState / 100.0) * timeDelta;
            }
            else
            {
                return 0;
            }
        }

        private float CalculateSpeed(float currentSpeed, double netAcceleration, double timeDelta)
        {
            double newSpeed = currentSpeed + netAcceleration * timeDelta * 3.6; // Convert m/s to km/h
            return (float)Math.Max(0, newSpeed); // Ensure the speed doesn't go below 0
        }

        private float CalculateRPM(float speed)
        {
            double speedInMetersPerSecond = speed / 3.6; // Convert km/h to m/s
            double wheelRotationsPerSecond = speedInMetersPerSecond / WheelCircumference;
            return (float)(wheelRotationsPerSecond * 60); // Convert wheel rotations per second to RPM
        }
    }
}
