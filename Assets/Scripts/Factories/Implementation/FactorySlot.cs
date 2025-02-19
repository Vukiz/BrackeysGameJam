using System;
using Factories.Infrastructure;
using Factories.Views;
using Rails.Infrastructure;
using UnityEngine;

namespace Factories.Implementation
{
    public class FactorySlot : IWaypoint
    {
        private FactorySlotView _view;

        public Action<IRobot> RobotReachedSlot;

        public void Initialize(FactorySlotView view)
        {
            _view = view;
        }
        
        public Vector3 Position => _view.transform.position;
        public void Reach(IRobot robot)
        {
            RobotReachedSlot?.Invoke(robot);
        }

        public void AddNeighbour(IWaypoint waypoint)
        {
                        
        }
    }
}