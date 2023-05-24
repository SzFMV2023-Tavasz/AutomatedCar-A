namespace AutomatedCar.SystemComponents.Packets
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class LKANotifierPacket : ILKANotifierPacket
    {
        public bool Intervention { get; set; }
    }
}
