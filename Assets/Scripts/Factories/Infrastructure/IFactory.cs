using Factories.Data;
using Factories.Views;
using Rails.Infrastructure;
using UnityEngine;

namespace Factories.Infrastructure
{
    public interface IFactory
    {
        void Initialize(FactoryView view, FactoryData data, IWaypoint next, Transform robotsParent);
        float SpawnProgress { get; }
        void SetPaused(bool isPaused);
    }
}