using System;
using Factories.Infrastructure;
using Rails.Infrastructure;

namespace Workstation.Infrastructure
{
    public interface ISlot : IWaypoint
    {
        bool IsOccupied { get; }
        IRobot RobotAssigned { get; set; }
        IRobot OccupiedBy { get; }

        event Action<ISlot> Occupied;
        event Action<IRobot, IRobot, ISlot> RobotsCollided;
        void SetDestroyed(bool b);
    }
}