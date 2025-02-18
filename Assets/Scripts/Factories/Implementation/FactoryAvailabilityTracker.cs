using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using Factories.Infrastructure;
using Factories.View;
using Level.Implementation;
using Orders;
using SushiBelt.Infrastructure;
using UnityEngine;

namespace Factories.Implementation
{
    public class FactoryAvailabilityTracker : IFactoryAvailabilityTracker, IDisposable
    {
        private readonly FactoryProvider _factoryProvider;
        private readonly CollisionsTracker _collisionsTracker;

        private readonly List<ISushiBelt> _sushiBelts = new();
        private readonly Dictionary<WorkType, IFactory> _factories = new();
        private readonly Queue<WorkType> _requiredFactoriesQueue = new();
        private List<FactorySlotView> _factorySlots;
        private Transform _factoriesParent;
        private Transform _robotsParent;
        private CancellationTokenSource _cancellationTokenSource;
        private FactorySlotView _selectedFactorySlotView;

        public FactoryAvailabilityTracker(FactoryProvider factoryProvider)
        {
            _factoryProvider = factoryProvider;
        }

        public void Initialize(List<FactorySlotView> factorySlots, Transform factoriesParent, Transform robotsParent)
        {
            _factoriesParent = factoriesParent;
            _robotsParent = robotsParent;
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
            while (!_selectedFactorySlotView)
            {
                token.ThrowIfCancellationRequested();
                await UniTask.Yield();
            }
            
            _factorySlots.Remove(_selectedFactorySlotView);
            _selectedFactorySlotView.SlotSelected -= OnSlotSelected;
            var factory = _factoryProvider.Create(workType, _selectedFactorySlotView.transform.position,
                _selectedFactorySlotView.NextWaypointView, _factoriesParent, _robotsParent);
                    
            _factories.Add(workType, factory);
            _selectedFactorySlotView = null;
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

        private void OnSlotSelected(FactorySlotView factorySlotView)
        {
            _selectedFactorySlotView = factorySlotView;
        }
    }
}