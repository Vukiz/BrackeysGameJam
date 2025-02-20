using System.Collections.Generic;
using Factories.Views;
using SushiBelt;
using SushiBelt.Infrastructure;
using UnityEngine;

namespace Factories.Infrastructure
{
    public interface IFactoryAvailabilityTracker
    {
        bool IsPaused { set; }
        void Initialize(List<FactorySlotView> factorySlots, Transform factoriesParent, Transform robotsParent);
        void RegisterSushiBeltForTracking(ISushiBelt sushiBelt);
        void Reset();
        // subscribe to all sushibelts  and allow user to place a factory into an empty factory slot
    }
}