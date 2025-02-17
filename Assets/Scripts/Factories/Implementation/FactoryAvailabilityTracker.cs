using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using Factories.Infrastructure;
using Factories.View;
using Orders;
using SushiBelt.Infrastructure;
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
        private CancellationTokenSource _cancellationTokenSource;
        private FactorySlot _selectedFactorySlot;

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
            foreach (var factorySlot in _factorySlots)
            {
                factorySlot.SlotSelected += OnSlotSelected;
            }
        }

        public void RegisterSushiBeltForTracking(ISushiBelt sushiBelt)
        {
            _sushiBelts.Add(sushiBelt);
            sushiBelt.OrderReceived += OnOrderReceived;
        }

        public void Dispose()
        {
            Unsubscribe();
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource?.Dispose();
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

                _cancellationTokenSource?.Cancel();
                _cancellationTokenSource?.Dispose();
                _cancellationTokenSource = new CancellationTokenSource();
                var token = _cancellationTokenSource.Token;
                SetFactoriesPaused(true); // EA: I don't think it is needed to pause factories here
                SetFactorySlotsInteractable(true);
                await RequestFactoryPlacement(token);
                SetFactoriesPaused(false);
                SetFactorySlotsInteractable(false);
            }
            catch (OperationCanceledException e)
            {
                Debug.Log(e);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }

        private async UniTask RequestFactoryPlacement(CancellationToken token)
        {
            while (_requiredFactoriesQueue.TryDequeue(out var workType))
            {
                await HandleConcreteWorkType(workType, token);
            }
        }

        private async UniTask HandleConcreteWorkType(WorkType workType, CancellationToken token)
        {
            while (!_selectedFactorySlot)
            {
                token.ThrowIfCancellationRequested();
                await UniTask.Yield();
            }
            
            _factorySlots.Remove(_selectedFactorySlot);
            _selectedFactorySlot.SlotSelected -= OnSlotSelected;
            var factory = _factoryProvider.Create(workType, _selectedFactorySlot.transform.position,
                _selectedFactorySlot.NextWaypointView);
                    
            _factories.Add(workType, factory);
            _selectedFactorySlot = null;
        }

        private void Unsubscribe()
        {
            foreach (var sushiBelt in _sushiBelts)
            {
                sushiBelt.OrderReceived -= OnOrderReceived;
            }

            if (_factorySlots == null)
            {
                return;
            }
            
            foreach (var factorySlot in _factorySlots)
            {
                factorySlot.SlotSelected -= OnSlotSelected;
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

        private void SetFactorySlotsInteractable(bool isInteractable)
        {
            foreach (var factorySlot in _factorySlots)
            {
                factorySlot.SetInteractable(isInteractable);
            }
        }

        private void OnSlotSelected(FactorySlot factorySlot)
        {
            _selectedFactorySlot = factorySlot;
        }
    }
}