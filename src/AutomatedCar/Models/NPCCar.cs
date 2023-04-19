namespace AutomatedCar.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Avalonia.Media;
    using global::AutomatedCar.NPC;
    using static global::AutomatedCar.Models.World;

    public class NPCCar : Car, INPC
    {
        private NPCManager nPCManager;

        public WorldType WorldType { get; set; }
        public NPCCar(int x, int y, string filename, WorldType type, NPCManager nPCManager)
            : base(x, y, filename)
        {
            this.nPCManager = nPCManager;
            this.WorldType = type;
            nPCManager.AddNPC(this);
        }
        public PolylineGeometry Geometry { get; set; }

        public List<Helpers.PathPoint> PathPoints { get; set; } = new List<Helpers.PathPoint>();

        public int ActPoint { get; set; } = 0;

        public bool Repeating { get; set; } = true;

        public void Move()
        {
            if (WorldType == WorldType.Oval && ActPoint == PathPoints.Count()-1)
            {
                this.Repeating = false;// Stop if completed all the pathpoints
                return;
            }
            if (!(ActPoint == PathPoints.Count() - 1 && !Repeating))
            {
                int NextPoint = ActPoint + 1;
                if (!(NextPoint < PathPoints.Count())) { NextPoint = 0; }
                int difX = PathPoints[NextPoint].X - this.X;
                int difY = PathPoints[NextPoint].Y - this.Y;
                double distance = Math.Sqrt(difX * difX + difY * difY);
                if ((int)Math.Floor(distance) <= this.Speed)
                {
                    this.X = PathPoints[NextPoint].X;
                    this.Y = PathPoints[NextPoint].Y;
                    this.Speed = PathPoints[NextPoint].Speed;
                    //this.Rotation = PathPoints[NextPoint].Rotation;
                    this.ActPoint = NextPoint;
                }
                else
                {
                    this.Speed = PathPoints[NextPoint].Speed;
                    double distancePerSpeedRatio = distance / this.Speed;
                    this.X += (int)Math.Round(difX / distancePerSpeedRatio);
                    this.Y += (int)Math.Round(difY / distancePerSpeedRatio);
                    this.RotateTowardsNextPoint();
                }
            }
        }

        private void RotateTowardsNextPoint()
        {
            // Check if there is a next point in the path
            if (this.ActPoint + 1 >= this.PathPoints.Count) return;

            // Get the rotation of the next point in the path
            var nextPoint = this.PathPoints[this.ActPoint + 1];
            double nextRotation = nextPoint.Rotation;

            // Adjust the next rotation if necessary
            // There is a bug, if the next rotation is 0 the rotationdifference is going to be negative so the car will always rotate left.
            if (nextRotation == 0 && this.Rotation >= 270)
            {
                nextRotation = 359;
            }

            // Calculate the rotation difference and rotation per tick
            double rotationDifference = nextRotation - this.Rotation;

            if (rotationDifference < 0) 
            {
                rotationDifference = 1;
            }
            double rotationPerTick = Math.Abs(rotationDifference * this.Speed / 60);

            // Rotate the object towards the next point
            this.Rotation += (rotationDifference < 0) ? -rotationPerTick : rotationPerTick;
        }
    }
}
