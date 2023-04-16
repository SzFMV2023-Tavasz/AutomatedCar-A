namespace AutomatedCar.Models
{
    using System;
    using System.Collections.Generic;
    using global::AutomatedCar.NPC;

    public class Pedestrian : INPC
    {
        private NPCManager nPCManager;

        public Pedestrian(NPCManager nPCManager)
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
