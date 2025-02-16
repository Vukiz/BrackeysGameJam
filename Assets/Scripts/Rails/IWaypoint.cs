using Factories;
using UnityEngine;

namespace Rails
{
    public interface IWaypoint
    {
        IWaypoint Next { get; }
        Vector3 Position { get; } // waypoint current position

        void Reach(IRobot robot); // robot reaches the waypoint
    }
}