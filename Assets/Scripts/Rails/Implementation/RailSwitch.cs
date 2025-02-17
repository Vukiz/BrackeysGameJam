using System.Collections.Generic;
using Factories;
using Rails.View;
using UnityEngine;

namespace Rails.Implementation
{
    public class RailSwitch : IWaypoint
    {
        private readonly List<IWaypoint> _neighbourWaypoints = new List<IWaypoint>();
        private RailSwitchView _view;
        public IWaypoint Next { get; }
        public Vector3 Position => _view.transform.position;

        public void Initialize(RailSwitchView view)
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
        }
    }
}