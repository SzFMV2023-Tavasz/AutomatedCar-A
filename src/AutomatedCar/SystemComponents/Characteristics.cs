namespace AutomatedCar.SystemComponents
{
    using AutomatedCar.SystemComponents.Packets;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// The characteristics class, which calculates the RPM and the speed based on the car's characteristic.
    /// </summary>
    public class Characteristics : SystemComponent
    {
        private ICharacteristicsInterface characteristicsPacket;

        /// <summary>
        /// Initializes a new instance of the <see cref="Characteristics"/> class.
        /// </summary>
        /// <param name="virtualFunctionBus"> The virtual function bus. </param>
        public Characteristics(VirtualFunctionBus virtualFunctionBus)
            : base(virtualFunctionBus)
        {
            this.characteristicsPacket = new CharacteristicsPacket();
            virtualFunctionBus.CharacteristicsPacket = this.characteristicsPacket;
        }

        /// <summary>
        /// Calculates the actual RPM and Speed based on the characteristic.
        /// </summary>
        public override void Process()
        {
            throw new NotImplementedException();
        }
    }
}
