using Factories;
using UnityEngine;

namespace Rails
{
    public interface IWaypoint
    {
        Vector3 Position { get; } // waypoint current position

        void Reach(IRobot robot); // robot reaches the waypoint
        void AddNeighbour(IWaypoint waypoint);
    }
}