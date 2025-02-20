using System;
using Factories.Infrastructure;
using Rails.Infrastructure;
using UnityEngine;
using Workstation.Infrastructure;
using Workstation.Views;

namespace Workstation.Implementation
{
    public class Slot : ISlot
    {
        public bool IsOccupied { get; private set; }
        public IRobot RobotAssigned { get; set; }
        public event Action<ISlot> Occupied;
        public event Action<IRobot, IRobot, ISlot> RobotsCollided;
        public void SetDestroyed(bool isDestroyed)
        {
            var slotView = _slotView;
            slotView.SetDestroyed(isDestroyed);
        }

        private SlotView _slotView;
        public IRobot OccupiedBy { get; private set; }

        public void SetView(SlotView slotView)
        {
            _slotView = slotView;
            slotView.SetDestroyed(false);
        }

        public void Occupy(IRobot robot)
        {
            OccupiedBy = robot;
            IsOccupied = true;
            robot.RobotDestroyRequested += OnRobotDestroyed;
            Occupied?.Invoke(this);
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
            RobotAssigned = null;
            IsOccupied = false;
            OccupiedBy = null;
        }

        public void AddNeighbour(IWaypoint waypoint)
        {
            
        }

        private void OnRobotDestroyed(IRobot robot)
        {
            robot.RobotDestroyRequested -= OnRobotDestroyed;
            Reset();
        }
    }
}