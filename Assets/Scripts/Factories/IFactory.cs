using Orders;

namespace Factories
{
    public interface IFactory
    {
        WorkType WorkType { get; } // Type of the robot that factory spawns
    }
}