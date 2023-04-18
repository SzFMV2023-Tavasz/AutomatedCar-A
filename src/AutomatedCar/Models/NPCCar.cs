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
                    this.Rotation = PathPoints[NextPoint].Rotation;
                    ActPoint = NextPoint;
                }
                else
                {
                    this.Speed = PathPoints[NextPoint].Speed;
                    double distancePerSpeedRatio = distance / this.Speed;
                    this.X += (int)Math.Round(difX / distancePerSpeedRatio);
                    this.Y += (int)Math.Round(difY / distancePerSpeedRatio);
                }
            }
        }
    }
}
