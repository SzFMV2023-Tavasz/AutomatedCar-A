namespace AutomatedCar.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    internal class AdaptiveTempomatInfo
    {
        public class DetectedObjectInfo
        {
            public WorldObject DetectedObject { get; set; }

            public bool isActive{ get; set; }


            public override bool Equals(object obj)
            {
                return (obj is DetectedObjectInfo) && (obj as DetectedObjectInfo).DetectedObject.Equals(this.DetectedObject);
            }

            public override int GetHashCode()
            {
                return this.DetectedObject.GetHashCode();
            }

            public override string ToString()
            {
                return DetectedObject.Filename;
            }
        }
    }
}
