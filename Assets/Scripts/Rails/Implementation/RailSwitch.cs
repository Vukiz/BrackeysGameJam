using System.Collections.Generic;
using Factories;
using Factories.Infrastructure;
using Rails.Infrastructure;
using Rails.View;
using UnityEngine;

namespace Rails.Implementation
{
    public class RailSwitch : IWaypoint
    {
        private readonly List<IWaypoint> _neighbourWaypoints = new();
        private RailSwitchView _view;
        private int _currentNeighbourIndex;
        private IWaypoint Next => _neighbourWaypoints[_currentNeighbourIndex];
        public Vector3 Position => _view.transform.position;

        public void SetView(RailSwitchView view)
        {
            _view = view;
            _view.InteractionRequired += OnInteractionRequired;
        }
        
        public void Reach(IRobot robot)
        {
            robot.SetNextWaypoint(Next);
        }

        public void AddNeighbour(IWaypoint waypoint)
        {
            _neighbourWaypoints.Add(waypoint); // TODO: Sort by angle from Vector3.forward
        }

        private void OnInteractionRequired()
        {
            // TODO: Add rotation logic
            _currentNeighbourIndex = (_currentNeighbourIndex + 1) % _neighbourWaypoints.Count;
        }
    }
}