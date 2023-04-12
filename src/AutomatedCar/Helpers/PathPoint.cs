namespace AutomatedCar.Helpers
{
    public class PathPoint
    {
        private int x;
        private int y;
        private double rotation;
        private int speed;

        public PathPoint(int x, int y, double rotation, int speed)
        {
            this.x = x;
            this.y = y;
            this.rotation = rotation;
            this.speed = speed;
        }

        public int X { get { return this.x; } }

        public int Y { get { return this.y; } }

        public double Rotation { get { return this.rotation; } }

        public int Speed { get { return this.speed; } }

        public PathPoint NextPoint { get; set; }
    }
}
