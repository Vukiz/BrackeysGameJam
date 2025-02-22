using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Factories.Infrastructure;
using Factories.Views;
using Level.Infrastructure;
using Orders.Data;
using Orders.Infrastructure;
using SushiBelt.Infrastructure;
using UnityEngine;
using Zenject;

namespace Factories.Implementation
{
    public class FactoryAvailabilityTracker : IFactoryAvailabilityTracker, IDisposable, ITickable
    {
        private readonly FactoryProvider _factoryProvider;
        private readonly IWaypointProvider _waypointProvider;
        private readonly List<ISushiBelt> _sushiBelts = new();
        private readonly Queue<WorkType> _requiredFactoriesQueue = new();
        private List<FactorySlotView> _availableFactorySlots = new();
        private Transform _factoriesParent;
        private Transform _robotsParent;
        private FactorySlotView _selectedFactorySlotView;

        private readonly HashSet<WorkType> _coveredWorkTypes = new();

        public FactoryAvailabilityTracker(
            FactoryProvider factoryProvider,
            IWaypointProvider waypointProvider
        )
        {
            _factoryProvider = factoryProvider;
            _waypointProvider = waypointProvider;
        }

        public bool IsPaused { set; private get; }

        public void Initialize(List<FactorySlotView> factorySlots, Transform factoriesParent, Transform robotsParent)
        {
            _factoriesParent = factoriesParent;
            _robotsParent = robotsParent;
            Unsubscribe();
            _availableFactorySlots = factorySlots;
            _sushiBelts.Clear();
            _coveredWorkTypes.Clear();
            _requiredFactoriesQueue.Clear();
            IsPaused = false;
            foreach (var factorySlot in _availableFactorySlots)
            {
                factorySlot.SlotSelected += OnSlotSelected;
            }
        }

        public void Tick()
        {
            if (IsPaused)
            {
                return;
            }

            if (_requiredFactoriesQueue.Count == 0)
            {
                return;
            }

            if (_selectedFactorySlotView == null)
            {
                SetFactorySlotsInteractable(true);
                return;
            }

            var workType = _requiredFactoriesQueue.Dequeue();
            _availableFactorySlots.Remove(_selectedFactorySlotView);
            _selectedFactorySlotView.SetInteractable(false);
            _selectedFactorySlotView.SlotSelected -= OnSlotSelected;
            var nextWaypoint = _waypointProvider.GetWaypoint(_selectedFactorySlotView.NextWaypointView);

            var factory = _factoryProvider.Create(workType,
                _selectedFactorySlotView.transform.position,
                nextWaypoint,
                _factoriesParent,
                _robotsParent
            );
            factory.StartCycle().Forget();

            _selectedFactorySlotView = null;
            SetFactorySlotsInteractable(false);
        }

        public void RegisterSushiBeltForTracking(ISushiBelt sushiBelt)
        {
            _sushiBelts.Add(sushiBelt);
            sushiBelt.OrderReceived += OnOrderReceived;
        }

        public void Reset()
        {
            _sushiBelts.Clear();
            _coveredWorkTypes.Clear();
            _requiredFactoriesQueue.Clear();
            Unsubscribe();
        }

        public void Dispose()
        {
            Unsubscribe();
        }

        private void OnOrderReceived(IOrder order)
        {
            foreach (var workType in order.NeededTypes)
            {
                if (!_coveredWorkTypes.Add(workType))
                {
                    continue;
                }

                _requiredFactoriesQueue.Enqueue(workType);
                SetFactorySlotsInteractable(true);
            }
        }

        private void Unsubscribe()
        {
            foreach (var sushiBelt in _sushiBelts)
            {
                sushiBelt.OrderReceived -= OnOrderReceived;
            }

            foreach (var factorySlot in _availableFactorySlots)
            {
                factorySlot.SlotSelected -= OnSlotSelected;
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