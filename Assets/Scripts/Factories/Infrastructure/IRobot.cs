using System;
using Orders;
using Rails.Infrastructure;
using UnityEngine;

namespace Factories.Infrastructure
{
    public interface IRobot
    {
        WorkType WorkType { get; }
        Vector3 Position { get; }
        bool IsTrackingRequired { get; set; }
        
        void SetNextWaypoint(IWaypoint waypoint, IIntermediateWaypoint intermediateWaypoint = null);
        
        void CompleteOrder(IOrder order);

        event Action CollisionDetected;
        event Action<IRobot> RobotDestroyRequested;
        void Destroy();
    }
}