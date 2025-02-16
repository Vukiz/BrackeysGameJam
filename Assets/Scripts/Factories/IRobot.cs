using Orders;
using Rails;

namespace Factories
{
    public interface IRobot
    {
        WorkType WorkType { get; }
        
        void SetNextWaypoint(IWaypoint waypoint);
        
        void CompleteOrder(IOrder order);

        event System.Action CollisionDetected;
    }
}