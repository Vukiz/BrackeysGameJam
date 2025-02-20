using System;
using Factories.Infrastructure;
using Rails.Infrastructure;

namespace Workstation.Infrastructure
{
    public interface ISlot : IWaypoint
    {
        bool IsOccupied { get; }

        IRobot OccupiedBy { get; }

        void Reset();

        event Action Occupied;
        event Action<IRobot, IRobot, ISlot> RobotsCollided;
    }
}