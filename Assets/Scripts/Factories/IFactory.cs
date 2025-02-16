using Orders;
using Rails;

namespace Factories
{
    public interface IFactory : IWaypoint
    {
        WorkType WorkType { get; } // Type of the robot that factory spawns
        event System.Action<IRobot> RobotSpawned;
        void PauseCycle();
        void ResumeCycle();
    }
}