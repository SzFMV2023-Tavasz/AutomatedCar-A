namespace AutomatedCar.Helpers
{
    public class PathPoint
    {
        private int x;
        private int y;
        private double rotation;
        private int speed;
        private int nextPoint;

        public PathPoint(int x, int y, double rotation, int speed, int nextPoint)
        {
            this.x = x;
            this.y = y;
            this.rotation = rotation;
            this.speed = speed;
            this.nextPoint = nextPoint;
        }

        public int X { get => this.x; }

        public int Y { get => this.y; }

        public double Rotation { get => this.rotation; }

        public int Speed { get => this.speed; }

        public int NextPoint { get => this.nextPoint; }
    }
}
