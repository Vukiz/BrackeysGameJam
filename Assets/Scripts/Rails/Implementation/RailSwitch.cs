using System.Collections.Generic;
using Factories.Infrastructure;
using Rails.Infrastructure;
using Rails.Views;
using UnityEngine;

namespace Rails.Implementation
{
    public class RailSwitch : IRailSwitch
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
            if (!_view)
            {
                return;
            }
            
            robot.SetNextWaypoint(Next);
        }

        public void AddNeighbour(IWaypoint waypoint)
        {
            _neighbourWaypoints.Add(waypoint);
        }

        private void OnInteractionRequired()
        {
            _currentNeighbourIndex = (_currentNeighbourIndex + 1) % _neighbourWaypoints.Count;
            var position = Next.Position - _view.transform.position;
            _view.transform.rotation = Quaternion.LookRotation(position, Vector3.up);
            _view.transform.rotation = Quaternion.Euler(0, _view.transform.rotation.eulerAngles.y, 0);
        }
    }
}