namespace AutomatedCar.SystemComponents
{
    using AutomatedCar.Models;
    using Avalonia;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class CameraSensor:GenericSensor
    {
        public CameraSensor(SensorSettings sensorSettings) 
            : base(sensorSettings)
        {
            sensorSettings.FunctionBus.CameraPacket = this.Packet;
        }
    }
}