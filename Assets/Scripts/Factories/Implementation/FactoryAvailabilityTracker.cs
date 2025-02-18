using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using Factories.Infrastructure;
using Factories.View;
using Factories.Views;
using Level.Implementation;
using Level.Infrastructure;
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
        private List<FactorySlotView> _availableFactorySlots;
        private Transform _factoriesParent;
        private Transform _robotsParent;
        private CancellationTokenSource _cancellationTokenSource;
        private FactorySlotView _selectedFactorySlotView;

        private readonly HashSet<WorkType> _coveredWorkTypes = new();

        public FactoryAvailabilityTracker(
            FactoryProvider factoryProvider
        )
        {
            _factoryProvider = factoryProvider;
        }

        public void Initialize(List<FactorySlotView> factorySlots, Transform factoriesParent, Transform robotsParent)
        {
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource = new CancellationTokenSource();
            _factoriesParent = factoriesParent;
            _robotsParent = robotsParent;
            Unsubscribe();
            _availableFactorySlots = factorySlots;
            _sushiBelts.Clear();
            _factories.Clear();
            _coveredWorkTypes.Clear();
            _requiredFactoriesQueue.Clear();
            foreach (var factorySlot in _availableFactorySlots)
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
        }

        private async void OnOrderReceived(IOrder order)
        {
            foreach (var workType in order.NeededTypes)
            {
                if (_coveredWorkTypes.Contains(workType))
                {
                    continue;
                }

                _requiredFactoriesQueue.Enqueue(workType);
            }

            if (!_requiredFactoriesQueue.Any())
            {
                return;
            }

            try
            {
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
                if (token.IsCancellationRequested)
                {
                    return;
                }
            }
        }

        private async UniTask HandleConcreteWorkType(WorkType workType, CancellationToken token)
        {
            await UniTask.WaitUntil(() => _selectedFactorySlotView, cancellationToken: token);
            if (token.IsCancellationRequested)
            {
                return;
            }

            _availableFactorySlots.Remove(_selectedFactorySlotView);
            _selectedFactorySlotView.SlotSelected -= OnSlotSelected;
            var factory = _factoryProvider.Create(workType,
                _selectedFactorySlotView.transform.position,
                _selectedFactorySlotView.NextWaypointView,
                _factoriesParent,
                _robotsParent
            );

            _factories.Add(workType, factory);
            _coveredWorkTypes.Add(workType);
            _selectedFactorySlotView = null;
        }

        private void Unsubscribe()
        {
            foreach (var sushiBelt in _sushiBelts)
            {
                sushiBelt.OrderReceived -= OnOrderReceived;
            }

            if (_availableFactorySlots == null)
            {
                return;
            }

            foreach (var factorySlot in _availableFactorySlots)
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
            foreach (var factorySlot in _availableFactorySlots)
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