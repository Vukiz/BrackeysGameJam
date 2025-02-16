using Factories;
using Rails;

namespace Workstation
{
    public interface ISlot : IWaypoint
    {
        bool IsOccupied { get; }
        
        IRobot OccupiedBy { get; }
        
        event System.Action Occupied;
    }
}