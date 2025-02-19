using System;
using Factories.Implementation;
using Factories.Infrastructure;
using Workstation.Infrastructure;

namespace Level.Infrastructure
{
    public interface ICollisionsTracker
    {
        void TrackCollisions();
        void Reset();
        void RegisterFactorySlot(FactorySlot factorySlot);
        void RegisterRobot(IRobot robot);
        void RegisterWorkstation(IWorkstation workstation);
        void RemoveRobot(IRobot robot);
        
        event Action RobotCollisionDetected; 
    }
}