using Orders;
using Rails;

namespace Factories
{
    public interface IRobot
    {
        WorkType WorkType { get; }
        
        void SetNextWaypoint(IWaypoint waypoint, IIntermediateWaypoint intermediateWaypoint = null);
        
        void CompleteOrder(IOrder order);

        event System.Action CollisionDetected;
    }
}