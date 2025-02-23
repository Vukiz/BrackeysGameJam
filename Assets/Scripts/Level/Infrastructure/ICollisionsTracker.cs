using System;
using Factories.Infrastructure;

namespace Level.Infrastructure
{
    public interface ICollisionsTracker
    {
        void TrackCollisions();
        void Reset();
        void RegisterRobot(IRobot robot);
        void RemoveRobot(IRobot robot);
        
        event Action<IRobot> RobotCollisionDetected; 
    }
}