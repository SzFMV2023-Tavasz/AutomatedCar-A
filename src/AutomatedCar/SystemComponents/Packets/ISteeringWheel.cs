namespace AutomatedCar.SystemComponents.Packets
{
    using AutomatedCar.Models;

    public interface ISteeringWheel
    {
        Vector2D DirectionVector { get; }
    }
}