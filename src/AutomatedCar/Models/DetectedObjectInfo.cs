namespace AutomatedCar.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class DetectedObjectInfo
    {
        public WorldObject DetectedObject { get; set; }

        public float Distance { get; set; }


        public override bool Equals(object obj)
        {
            return (obj is DetectedObjectInfo) && (obj as DetectedObjectInfo).DetectedObject.Equals(this.DetectedObject);
        }

        public override int GetHashCode()
        {
            return this.DetectedObject.GetHashCode();
        }
    }
}
