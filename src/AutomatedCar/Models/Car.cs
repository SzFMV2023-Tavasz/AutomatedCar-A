namespace AutomatedCar.Models
{
    public class Car : WorldObject
    {
        public Car(float x, float y, string filename)
            : base(x, y, filename)
        {
        }

        /// <summary>Gets or sets Speed in px/s.</summary>
        public int Speed { get; set; }
    }
}