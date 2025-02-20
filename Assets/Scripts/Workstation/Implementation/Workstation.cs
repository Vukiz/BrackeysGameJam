using System;
using System.Collections.Generic;
using System.Linq;
using Factories.Infrastructure;
using Level.Implementation;
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
        private ISushiBelt _sushiBelt;
        private WorkstationView _workstationView;

        public event Action<IRobot> RobotReachedStationWithNoEmptySlots;

        public Workstation(
            CollisionsTracker collisionsTracker,
            IVFXManager vfxManager
        )
        {
            _collisionsTracker = collisionsTracker;
            _vfxManager = vfxManager;
        }
        
        public void Cleanup()
        {
            foreach (var slot in _slots)
            {
                slot.Occupied -= OnSlotOccupied;
                slot.RobotsCollided -= OnRobotsCollided;
            }
            _sushiBelt.OrderReceived -= OnOrderReceived;
            _sushiBelt.OrderCompleted -= OnOrderCompleted;
            _sushiBelt.Cleanup();
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

        private void OnSlotOccupied(ISlot slot)
        {
            if (_sushiBelt.CurrentOrder == null)
            {
                slot.OccupiedBy.StartSelfDestructionTimer();
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
                    occupiedBy.StopSelfDestructionTimer();
                    slot.Reset();
                    continue;
                }
                
                slot.OccupiedBy.StartSelfDestructionTimer();
            }
        }

        private IWaypoint TryGetNextSlot()
        {
            var emptySlot = _slots.FirstOrDefault(slot => !slot.IsOccupied);
            if (emptySlot != null)
            {
                return emptySlot;
            }

            if (!_slots.Any())
            {
                return null;
            }

            var lastSlot = _slots.Last();
            _slots.Remove(lastSlot);
            return lastSlot;
        }

        public Vector3 Position => _workstationView.Position;

        public void Reach(IRobot robot)
        {
            _collisionsTracker.RemoveRobot(robot);
            var nextSlot = TryGetNextSlot();
            Debug.Log($"Robot {robot} reached workstation {this} with next slot {nextSlot}, with position {nextSlot.Position} and workstation position {Position}");
            robot.SetNextWaypoint(nextSlot);
        }

        public void AddNeighbour(IWaypoint waypoint)
        {
        }

        private void OnRobotsCollided(IRobot robot, IRobot otherRobot, ISlot slot)
        {
            _slots.Remove(slot);
            slot.SetDestroyed(true);
            robot.Destroy();
            otherRobot.Destroy();
            if (_slots.Count == 0)
            {
                RobotReachedStationWithNoEmptySlots?.Invoke(robot);
            }
            else
            {
                _vfxManager.SpawnVFX(VFXType.Explosion,
                    robot.Position); // since we are not ending the game when slot is destroyed but only if there are no slots
            }
        }
    }
}