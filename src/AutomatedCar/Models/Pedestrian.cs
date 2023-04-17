namespace AutomatedCar.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using global::AutomatedCar.NPC;

    public class Pedestrian : WorldObject, INPC
    {
        private NPCManager nPCManager;

        public Pedestrian(int x, int y, string filename, NPCManager nPCManager, int Speed = 2)
            : base(x, y, filename)
        {
            this.nPCManager = nPCManager;
            this.Speed = Speed;
            nPCManager.AddNPC(this);
        }

        public List<Helpers.PathPoint> PathPoints { get; set; } = new List<Helpers.PathPoint>();

        public int Speed { get; set; } = 0;

        public int ActPoint { get; set; } = 0;

        public bool Repeating { get; set; } = true;

        public void Move()
        {
            if (!(ActPoint == PathPoints.Count() - 1 && !Repeating))
            {
                int NextPoint = ActPoint + 1;
                if (!(NextPoint < PathPoints.Count())) { NextPoint = 0; }
                int difX = PathPoints[NextPoint].X - this.X;
                int difY = PathPoints[NextPoint].Y - this.Y;
                double distance = Math.Sqrt(difX * difX + difY * difY);
                if (distance <= this.Speed)
                {
                    this.X = PathPoints[NextPoint].X;
                    this.Y = PathPoints[NextPoint].Y;
                    this.Speed = PathPoints[NextPoint].Speed;
                    this.Rotation = PathPoints[NextPoint].Rotation;
                    ActPoint = NextPoint;
                }
                else
                {
                    double distancePerSpeedRatio = distance / this.Speed;
                    this.X += (int)Math.Round(difX / distancePerSpeedRatio);
                    this.Y += (int)Math.Round(difY / distancePerSpeedRatio);
                }
            }
        }
    }
}
