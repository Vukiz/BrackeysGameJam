using System.Collections.Generic;
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
            _neighbourWaypoints.Add(waypoint);
            SortWaypoints();
        }

        private void OnInteractionRequired()
        {
            _currentNeighbourIndex = (_currentNeighbourIndex + 1) % _neighbourWaypoints.Count;
            _view.transform.rotation = Quaternion.LookRotation(Next.Position, Vector3.up);
        }

        private void SortWaypoints()
        {
            _neighbourWaypoints.Sort((left, right) => GetAngle(left) < GetAngle(right) ? 1 : -1);
        }

        private float GetAngle(IWaypoint waypoint)
        {
            return Vector3.Angle(Vector3.forward, waypoint.Position);
        }
    }
}