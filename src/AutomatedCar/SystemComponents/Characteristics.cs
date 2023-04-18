namespace AutomatedCar.SystemComponents
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// The characteristics class.
    /// </summary>
    public class Characteristics : SystemComponent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Characteristics"/> class.
        /// </summary>
        /// <param name="virtualFunctionBus"> The virtual function bus. </param>
        public Characteristics(VirtualFunctionBus virtualFunctionBus)
            : base(virtualFunctionBus)
        {

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
