using System.Collections.Generic;
using System.Linq;
using Factories.Infrastructure;
using Orders;
using Rails.Infrastructure;
using SushiBelt.Infrastructure;
using UnityEngine;
using Workstation.Infrastructure;
using Workstation.Views;

namespace Workstation.Implementation
{
    public class Workstation : IWorkstation
    {
        private List<ISlot> _slots;
        private ISushiBelt _sushiBelt;
        private WorkstationView _workstationView;

        public void SetView(WorkstationView workstationView, ISushiBelt sushiBelt)
        {
            _workstationView = workstationView;
            _slots = new List<ISlot>();
            foreach (var slotView in workstationView.SlotViews)
            {
                var slot = new Slot();
                slot.SetView(slotView);
                slot.Occupied += OnSlotOccupied; // TODO Unsubscribe
                _slots.Add(slot);
            }

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
            return _slots.FirstOrDefault(slot => !slot.IsOccupied);

            // NoSlotsLeft?.Invoke();
        }

        public Vector3 Position => _workstationView.Position;

        public void Reach(IRobot robot)
        {
            robot.SetNextWaypoint(TryGetNextSlot());
        }

        public void AddNeighbour(IWaypoint waypoint)
        {
            
        }
    }
}