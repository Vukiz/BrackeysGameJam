using Factories.Data;
using Factories.View;
using Rails.Infrastructure;
using UnityEngine;

namespace Factories.Infrastructure
{
    public interface IFactory
    {
        void Initialize(FactoryView view, FactoryData data, IWaypoint next, Transform robotsParent);
        event System.Action<IRobot> RobotSpawned;
        void SetPaused(bool isPaused);
    }
}