using Factories.Infrastructure;
using UnityEngine;

namespace Rails.Infrastructure
{
    public interface IWaypoint
    {
        Vector3 Position { get; } // waypoint current position

        void Reach(IRobot robot); // robot reaches the waypoint
        void AddNeighbour(IWaypoint waypoint);
    }
}