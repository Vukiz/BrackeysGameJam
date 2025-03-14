using System;
using System.Collections.Generic;
using System.Linq;
using Factories.Data;
using Factories.Infrastructure;
using Level.Implementation;
using Orders.Implementation;
using Orders.Infrastructure;
using Rails.Infrastructure;
using SushiBelt.Infrastructure;
using UnityEngine;
using VFX.Data;
using VFX.Infrastructure;
using Workstation.Infrastructure;
using Workstation.Views;

namespace Workstation.Implementation
{
    public class Workstation : IWorkstation
    {
        private readonly CollisionsTracker _collisionsTracker;
        private readonly IVFXManager _vfxManager;

        private List<ISlot> _slots;
        private WorkstationView _workstationView;

        public event Action<IRobot> RobotReachedStationWithNoEmptySlots;
        public event Action SlotRemoved;

        public Workstation(
            CollisionsTracker collisionsTracker,
            IVFXManager vfxManager
        )
        {
            _collisionsTracker = collisionsTracker;
            _vfxManager = vfxManager;
        }

        public void AddOrderToSushiBelt(Order order)
        {
            SushiBelt.SubmitOrder(order);
        }

        public void Cleanup()
        {
            foreach (var slot in _slots)
            {
                slot.Occupied -= OnSlotOccupied;
                slot.RobotsCollided -= OnRobotsCollided;
            }

            SushiBelt.OrderReceived -= OnOrderReceived;
            SushiBelt.OrderCompleted -= OnOrderCompleted;
            SushiBelt.Cleanup();
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
            SushiBelt = sushiBelt;
            SushiBelt.OrderReceived += OnOrderReceived;
            SushiBelt.OrderCompleted += OnOrderCompleted; // TODO Unsubscribe from all events?
        }

        public ISushiBelt SushiBelt { get; private set; }

        private void OnSlotOccupied(ISlot slot)
        {
            if (SushiBelt.CurrentOrder == null)
            {
                slot.OccupiedBy.StartSelfDestructionTimer();
                return;
            }

            TryCompleteOrder(SushiBelt.CurrentOrder);
        }

        private void OnOrderCompleted(IOrder order)
        {
            _vfxManager.SpawnVFX(VFXType.OrderComplete, SushiBelt.OrderPosition);
        }

        private void OnOrderReceived(IOrder order)
        {
            TryCompleteOrder(order);
        }

        private async void TryCompleteOrder(IOrder order)
        {
            foreach (var slot in _slots.ToList())
            {
                if (!slot.IsOccupied)
                {
                    continue;
                }

                var neededTypes = order.NeededTypes; // get non completed types
                var occupiedBy = slot.OccupiedBy;
                if (neededTypes.Contains(occupiedBy.WorkType))
                {
                    occupiedBy.StopSelfDestructionTimer();
                    await occupiedBy.CompleteOrder(order, SushiBelt.OrderPosition);
                    SushiBelt.MarkWorkTypeCompleted(occupiedBy.WorkType);
                    continue;
                }

                slot.OccupiedBy.StartSelfDestructionTimer();
            }
        }

        private ISlot TryGetNextSlot()
        {
            var emptySlot = _slots.FirstOrDefault(slot => !slot.IsOccupied && slot.RobotAssigned == null);
            if (emptySlot != null)
            {
                return emptySlot;
            }

            if (!_slots.Any())
            {
                return null;
            }

            Debug.Log("No empty slots, removing last slot because it is about to be destroyed.");
            var lastSlot = _slots.Last();
            _slots.Remove(lastSlot);
            return lastSlot;
        }

        public Vector3 Position => _workstationView.Position;

        public void Reach(IRobot robot)
        {
            _collisionsTracker.RemoveRobot(robot);
            var nextSlot = TryGetNextSlot();
            robot.SetNextWaypoint(nextSlot);
            nextSlot.RobotAssigned = robot;
        }

        public void AddNeighbour(IWaypoint waypoint)
        {
        }

        private void OnRobotsCollided(IRobot robot, IRobot otherRobot, ISlot slot)
        {
            _slots.Remove(slot);
            slot.SetDestroyed(true);
            robot.Destroy(DestroyReason.Collision);
            otherRobot.Destroy(DestroyReason.Collision);
            if (_slots.Count == 0)
            {
                RobotReachedStationWithNoEmptySlots?.Invoke(robot);
            }
            else
            {
                SlotRemoved?.Invoke();
                _vfxManager.SpawnVFX(VFXType.Explosion,
                    robot.Position); // since we are not ending the game when slot is destroyed but only if there are no slots
            }
        }
    }
}