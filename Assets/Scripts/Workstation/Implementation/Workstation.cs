using System;
using System.Collections.Generic;
using System.Linq;
using Factories.Infrastructure;
using Level.Implementation;
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
        private readonly CollisionsTracker _collisionsTracker;
        
        private List<ISlot> _slots;
        private ISushiBelt _sushiBelt;
        private WorkstationView _workstationView;

        public event Action NoSlotsLeft;

        public Workstation(CollisionsTracker collisionsTracker)
        {
            _collisionsTracker = collisionsTracker;
        }

        public void SetView(WorkstationView workstationView, ISushiBelt sushiBelt)
        {
            _workstationView = workstationView;
            _slots = new List<ISlot>();
            foreach (var slotView in workstationView.SlotViews)
            {
                var slot = new Slot();
                slot.SetView(slotView);
                slot.Occupied += OnSlotOccupied; // TODO Unsubscribe
                slot.RobotsCollided += OnRobotsCollided;
                _slots.Add(slot);
            }

            sushiBelt.SetView(workstationView.SushiBeltView);
            _sushiBelt = sushiBelt;
            _sushiBelt.OrderReceived += OnOrderReceived;
            _sushiBelt.OrderCompleted += OnOrderCompleted; // TODO Unsubscribe from all events?
        }

        private void OnSlotOccupied()
        {
            if (_sushiBelt.CurrentOrder == null)
            {
                return;
            }
            
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
                    slot.Reset();
                }
            }
        }

        private IWaypoint TryGetNextSlot()
        {
            var emptySlot = _slots.FirstOrDefault(slot => !slot.IsOccupied);
            if (emptySlot != null)
            {
                return emptySlot;
            }

            var lastSlot = _slots.Last();
            _slots.Remove(lastSlot);
            return lastSlot;
            // NoSlotsLeft?.Invoke();
        }

        public Vector3 Position => _workstationView.Position;

        public void Reach(IRobot robot)
        {
            _collisionsTracker.RemoveRobot(robot);
            robot.SetNextWaypoint(TryGetNextSlot());
        }

        public void AddNeighbour(IWaypoint waypoint)
        {
            
        }

        private void OnRobotsCollided(IRobot robot, IRobot otherRobot, ISlot slot)
        {
            _slots.Remove(slot);
            robot.Destroy();
            otherRobot.Destroy();
            if (_slots.Count == 0)
            {
                NoSlotsLeft?.Invoke();
            }
        }
    }
}