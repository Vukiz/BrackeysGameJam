using Orders;
using Rails;
using Rails.Infrastructure;

namespace Factories.Infrastructure
{
    public interface IFactory : IWaypoint
    {
        WorkType WorkType { get; } // Type of the robot that factory spawns
        event System.Action<IRobot> RobotSpawned;
        void PauseCycle();
        void ResumeCycle();
    }
}