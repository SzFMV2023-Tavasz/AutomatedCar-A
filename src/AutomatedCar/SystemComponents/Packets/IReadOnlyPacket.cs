namespace AutomatedCar.SystemComponents.Packets
{
    using AutomatedCar.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public interface IReadOnlyPacket<T>
    {
        IReadOnlyCollection<T> WorldObjectsDetected { get; set; }
    }
}