using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Orders;
using SushiBelt;
using UnityEngine;

namespace Factories.Implementation
{
    public class FactoryAvailabilityTracker : IDisposable
    {
        private readonly FactoryProvider _factoryProvider;
        private readonly List<ISushiBelt> _sushiBelts;
        
        private readonly Dictionary<WorkType, IFactory> _factories = new Dictionary<WorkType, IFactory>();
        private readonly Queue<WorkType> _requiredFactoriesQueue = new Queue<WorkType>();

        public FactoryAvailabilityTracker(FactoryProvider factoryProvider, List<ISushiBelt> sushiBelts)
        {
            _factoryProvider = factoryProvider;
            _sushiBelts = sushiBelts;
        }

        public void Initialize()
        {
            // TODO: Pass factory slots
            foreach (var sushiBelt in _sushiBelts)
            {
                sushiBelt.OrderReceived += OnOrderReceived;
            }
        }

        public void Dispose()
        {
            foreach (var sushiBelt in _sushiBelts)
            {
                sushiBelt.OrderReceived -= OnOrderReceived;
            }
        }
        
        private async void OnOrderReceived(IOrder order)
        {
            try
            {
                foreach (var workType in order.NeededTypes)
                {
                    var hashset = new HashSet<WorkType>();
                    if (!_factories.TryGetValue(workType, out _) && !hashset.Contains(workType))
                    {
                        _requiredFactoriesQueue.Enqueue(workType);
                        hashset.Add(workType);
                    }
                }

                if (!_requiredFactoriesQueue.Any())
                {
                    return;
                }
            
                foreach (var (_, factory) in _factories)
                {
                    factory.PauseCycle();
                }
            
                await RequestFactoryPlacement();
                foreach (var (_, factory) in _factories)
                {
                    factory.ResumeCycle();
                }
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }

        private async UniTask RequestFactoryPlacement()
        {
            // TODO: Add factory placement of a required types from queue 
        }
    }
}