using Cysharp.Threading.Tasks;
using Factories.Data;
using Factories.Views;
using Rails.Infrastructure;
using UnityEngine;

namespace Factories.Infrastructure
{
    public interface IFactory
    {
        UniTask StartCycle();
        UniTask<IRobot> SpawnRobot();
        void Initialize(FactoryView view, FactoryData data, IWaypoint next, Transform robotsParent);
        float SpawnProgress { get; }
    }
}