using System;
using System.Collections.Generic;
using System.Linq;
using Factories.Infrastructure;
using Factories.Views;
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
        private readonly List<ISushiBelt> _sushiBelts = new();
        private readonly Queue<WorkType> _requiredFactoriesQueue = new();
        private List<FactorySlotView> _availableFactorySlots = new();
        private Transform _factoriesParent;
        private Transform _robotsParent;
        private FactorySlotView _selectedFactorySlotView;

        private readonly HashSet<WorkType> _coveredWorkTypes = new();

        public FactoryAvailabilityTracker(
            FactoryProvider factoryProvider
        )
        {
            _factoryProvider = factoryProvider;
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
            var factory = _factoryProvider.Create(workType,
                _selectedFactorySlotView.transform.position,
                _selectedFactorySlotView.NextWaypointView,
                _factoriesParent,
                _robotsParent
            );
            factory.SetPaused(false);

            _coveredWorkTypes.Add(workType);
            _selectedFactorySlotView = null;
            SetFactorySlotsInteractable(false);
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

        private void OnOrderReceived(IOrder order)
        {
            foreach (var workType in order.NeededTypes.Where(workType => !_coveredWorkTypes.Contains(workType)))
            {
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