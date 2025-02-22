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
        UniTaskVoid SpawnRobot();
        void Initialize(FactoryView view, FactoryData data, IWaypoint next, Transform robotsParent);
        float SpawnProgress { get; }
    }
}