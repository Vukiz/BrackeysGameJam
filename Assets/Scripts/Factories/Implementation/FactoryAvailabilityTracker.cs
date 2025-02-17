using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Factories.View;
using Orders;
using SushiBelt;
using UnityEngine;

namespace Factories.Implementation
{
    public class FactoryAvailabilityTracker : IFactoryAvailabilityTracker, IDisposable
    {
        private readonly FactoryProvider _factoryProvider;

        private readonly List<ISushiBelt> _sushiBelts = new();
        private readonly Dictionary<WorkType, IFactory> _factories = new();
        private readonly Queue<WorkType> _requiredFactoriesQueue = new();
        private List<FactorySlot> _factorySlots;

        public FactoryAvailabilityTracker(FactoryProvider factoryProvider)
        {
            _factoryProvider = factoryProvider;
        }

        public void Initialize(List<FactorySlot> factorySlots)
        {
            Unsubscribe();
            _factorySlots = factorySlots;
            _sushiBelts.Clear();
            _factories.Clear();
            _requiredFactoriesQueue.Clear();
        }

        public void RegisterSushiBeltForTracking(ISushiBelt sushiBelt)
        {
            _sushiBelts.Add(sushiBelt);
            sushiBelt.OrderReceived += OnOrderReceived;
        }

        public void Dispose()
        {
            Unsubscribe();
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

                SetFactoriesPaused(true); // EA: I don't think it is needed to pause factories here

                foreach (var slot in _factorySlots)
                {
                    slot.SetSlotButtonEnabled(true);
                }

                await RequestFactoryPlacement();
                
                SetFactoriesPaused(false);

                foreach (var slot in _factorySlots)
                {
                    slot.SetSlotButtonEnabled(false);
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

        private void Unsubscribe()
        {
            foreach (var sushiBelt in _sushiBelts)
            {
                sushiBelt.OrderReceived -= OnOrderReceived;
            }
        }

        private void SetFactoriesPaused(bool isPaused)
        {
            foreach (var (_, factory) in _factories)
            {
                if (isPaused)
                {
                    factory.PauseCycle();
                }
                else
                {
                    factory.ResumeCycle();
                }
            }
        }
    }
}