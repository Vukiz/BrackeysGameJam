using Orders;
using Rails;
using Rails.Infrastructure;

namespace Factories.Infrastructure
{
    public interface IRobot
    {
        WorkType WorkType { get; }
        
        void SetNextWaypoint(IWaypoint waypoint, IIntermediateWaypoint intermediateWaypoint = null);
        
        void CompleteOrder(IOrder order);

        event System.Action CollisionDetected;
    }
}