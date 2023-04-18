namespace AutomatedCar.Models
{
    using System;
    using System.Collections.Generic;
    using global::AutomatedCar.NPC;

    public class Pedestrian : WorldObject, INPC
    {
        private NPCManager nPCManager;

        public Pedestrian(int x, int y, string filename, NPCManager nPCManager)
            : base(x, y, filename)
        {
            this.nPCManager = nPCManager;
            nPCManager.AddNPC(this);
        }

        public List<Helpers.PathPoint> PathPoints { get; set; } = new List<Helpers.PathPoint>();

        public void Move()
        {
            throw new NotImplementedException();
        }
    }
}
