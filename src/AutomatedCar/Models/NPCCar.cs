namespace AutomatedCar.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Avalonia.Media;
    using global::AutomatedCar.NPC;

    public class NPCCar : Car, INPC
    {
        private NPCManager nPCManager;

        public NPCCar(int x, int y, string filename, NPCManager nPCManager)
            : base(x, y, filename)
        {
            this.nPCManager = nPCManager;
            nPCManager.AddNPC(this);
        }
        public PolylineGeometry Geometry { get; set; }

        public List<Helpers.PathPoint> PathPoints { get; set; } = new List<Helpers.PathPoint>();

        public void Move()
        {
            throw new NotImplementedException();
        }
    }
}
