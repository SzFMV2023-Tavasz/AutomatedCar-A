namespace AutomatedCar.NPC
{
    using System.Collections.Generic;
    using AutomatedCar.Helpers;

    public interface INPC
    {
        List<PathPoint> PathPoints { get; set; }

        int ActPoint { get; set; }

        bool Repeating { get; set; }

        void Move();
    }
}
