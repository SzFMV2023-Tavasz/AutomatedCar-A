namespace AutomatedCar.SystemComponents
{
    using AutomatedCar.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public interface IReadOnlyPacket
    {
        IReadOnlyCollection<WorldObject> WorldObjectsDetected { get; }
    }
}
