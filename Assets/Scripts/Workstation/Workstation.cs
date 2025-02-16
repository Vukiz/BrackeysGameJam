using System;
using System.Collections.Generic;
using System.Linq;
using Factories;
using Orders;
using Rails;
using SushiBelt;
using UnityEngine;

namespace Workstation
{
    public class Workstation : IWorkstation
    {
        private List<ISlot> _slots;
        private ISushiBelt _sushiBelt;
        private WorkstationView _workstationView;

        public event Action NoSlotsLeft;

        public IWaypoint Next => TryGetNextSlot();

        public void SetView(WorkstationView workstationView)
        {
            _slots = new List<ISlot>();
            foreach (var slotView in workstationView.SlotViews)
            {
                var slot = new Slot();
                slot.SetView(slotView);
                slot.Occupied += OnSlotOccupied; // TODO Unsubscribe
                _slots.Add(slot);
            }

            var sushiBelt = new SushiBelt.SushiBelt();
            sushiBelt.SetView(workstationView.SushiBeltView);
            _sushiBelt = sushiBelt;
            _sushiBelt.OrderReceived += OnOrderReceived;
            _sushiBelt.OrderCompleted += OnOrderCompleted; // TODO Unsubscribe from all events?
        }

        private void OnSlotOccupied()
        {
            TryCompleteOrder(_sushiBelt.CurrentOrder);
        }

        private void OnOrderCompleted(IOrder order)
        {
            // TODO Order Completed VFX
        }

        private void OnOrderReceived(IOrder order)
        {
            TryCompleteOrder(order);
        }

        private void TryCompleteOrder(IOrder order)
        {
            foreach (var slot in _slots)
            {
                if (!slot.IsOccupied)
                {
                    continue;
                }

                var neededTypes = order.NeededTypes; // get non completed types
                var occupiedBy = slot.OccupiedBy;
                if (neededTypes.Contains(occupiedBy.WorkType))
                {
                    occupiedBy.CompleteOrder(order);
                }
            }
        }

        private IWaypoint TryGetNextSlot()
        {
            foreach (var slot in _slots.Where(slot => !slot.IsOccupied))
            {
                return slot;
            }

            NoSlotsLeft?.Invoke();
            return null;
        }

        public Vector3 Position => _workstationView.Position;

        public void Reach(IRobot robot)
        {
        }
    }
}