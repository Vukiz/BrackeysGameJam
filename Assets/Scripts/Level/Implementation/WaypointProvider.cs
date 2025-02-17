using System.Collections.Generic;
using Rails;
using Rails.Infrastructure;
using UnityEngine;

namespace Level.Implementation
{
    public class WaypointProvider
    {
        private readonly Dictionary<IWaypointView, IWaypoint> _waypoints = new();

        public void RegisterWaypoint(IWaypointView waypointView, IWaypoint waypoint)
        {
            if (_waypoints.TryAdd(waypointView, waypoint))
            {
                return;
            }

            Debug.LogError("Waypoint already has been added");
        }

        public IWaypoint GetWaypoint(IWaypointView waypointView)
        {
            if (_waypoints.TryGetValue(waypointView, out var waypoint))
            {
                return waypoint;
            }

            Debug.LogError("Waypoint not found");
            return null;

        }
    }
}