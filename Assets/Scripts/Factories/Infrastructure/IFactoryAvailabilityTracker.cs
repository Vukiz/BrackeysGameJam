using System.Collections.Generic;
using Factories.View;
using SushiBelt;
using SushiBelt.Infrastructure;
using UnityEngine;

namespace Factories.Infrastructure
{
    public interface IFactoryAvailabilityTracker
    {
        void Initialize(List<FactorySlotView> factorySlots, Transform factoriesParent, Transform robotsParent);
        void RegisterSushiBeltForTracking(ISushiBelt sushiBelt);
        // subscribe to all sushibelts  and allow user to place a factory into an empty factory slot
    }
}