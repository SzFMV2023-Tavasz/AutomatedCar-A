namespace AutomatedCar.SystemComponents
{
    using AutomatedCar.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Numerics;
    using System.Text;
    using System.Threading.Tasks;

    class CameraSensor : GenericSensor
    {
        IReadOnlyPacket packet;
        public CameraSensor(VirtualFunctionBus virtualFunctionBus,Vector2 carAnchorPoint,double fOV, double viewDistance,
        IEnumerable<WorldObjectType> worldObjectTypesFilter): base(virtualFunctionBus,carAnchorPoint,fOV,viewDistance,worldObjectTypesFilter)
        {
            this.CarAnchorPoint = new Vector2(World.Instance.WorldObjects[0].X,World.Instance.WorldObjects[1].Y); //TO DO eltolás
            this.FOV = 60.0;
            this.ViewDistance = 80.0*50;
            this.WorldObjectTypesFilter = new List<WorldObjectType> { WorldObjectType.Road,WorldObjectType.Crosswalk};
            this.virtualFunctionBus.SensorPacket = this.packet; //Itt teljesen elvagyok veszve
        }

        //To DO intersect függvény


        public override void Process()
        {
            throw new NotImplementedException();
        }
    }
}
