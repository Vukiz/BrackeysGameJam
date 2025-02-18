using System;
using Factories.Implementation;
using Factories.Infrastructure;

namespace Level.Infrastructure
{
    public interface ICollisionsTracker
    {
        void TrackCollisions();
        void Reset();
        void RegisterFactorySlot(FactorySlot factorySlot);
        void RegisterRobot(IRobot robot);
        void RemoveRobot(IRobot robot);
        
        event Action RobotCollisionDetected; 
    }
}