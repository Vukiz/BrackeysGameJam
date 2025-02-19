using System;
using Factories;
using Factories.Infrastructure;
using Rails;
using Rails.Infrastructure;
using UnityEngine;
using Workstation.Infrastructure;
using Workstation.Views;

namespace Workstation.Implementation
{
    public class Slot : ISlot
    {
        public bool IsOccupied { get; private set; }
        public event Action Occupied;
        
        private SlotView _slotView;
        public IRobot OccupiedBy { get; private set; }

        public event Action<IRobot, IRobot, ISlot> RobotsCollided;

        public void SetView(SlotView slotView)
        {
            _slotView = slotView;
        }

        private void Occupy(IRobot robot)
        {
            OccupiedBy = robot;
            IsOccupied = true;
            Occupied?.Invoke();
        }

        public Vector3 Position => _slotView.transform.position;

        public void Reach(IRobot robot)
        {
            if (OccupiedBy != null)
            {
                RobotsCollided?.Invoke(OccupiedBy, robot, this);
                _slotView.gameObject.SetActive(false);
                return;
            }
            
            Occupy(robot);
            // TODO VFX for robot reaching the slot
        }

        public void Reset()
        {
            IsOccupied = false;
            OccupiedBy = null;
        }

        public void AddNeighbour(IWaypoint waypoint)
        {
            
        }
    }
}