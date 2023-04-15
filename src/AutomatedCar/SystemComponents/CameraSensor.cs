namespace AutomatedCar.SystemComponents
{
    using AutomatedCar.Models;
    using Avalonia;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    class CameraSensor:GenericSensor
    {
        public CameraSensor(AutomatedCar car, VirtualFunctionBus virtualFunctionBus, Point carAnchorPoint, double fOV, double viewDistance, 
            IEnumerable<WorldObjectType> worldObjectTypesFilter) : base(car, virtualFunctionBus, carAnchorPoint, fOV, viewDistance, worldObjectTypesFilter)
        {
            virtualFunctionBus.CameraPacket = this.Packet;
        }
    }
}
