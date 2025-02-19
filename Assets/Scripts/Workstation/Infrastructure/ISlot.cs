using Factories;
using Factories.Infrastructure;
using Rails;
using Rails.Infrastructure;

namespace Workstation.Infrastructure
{
    public interface ISlot : IWaypoint
    {
        bool IsOccupied { get; }
        
        IRobot OccupiedBy { get; }

        void Reset();
        
        event System.Action Occupied;
    }
}