using System.Collections.Generic;
using Rails;
using UnityEngine;

namespace Level
{
    public class WaypointProvider
    {
        private readonly Dictionary<IWaypointView, IWaypoint> _waypoints = new Dictionary<IWaypointView, IWaypoint>();

        public void RegisterWaypoint(IWaypointView waypointView, IWaypoint waypoint)
        {
            if (!_waypoints.TryAdd(waypointView, waypoint))
            {
                Debug.LogError("Waypoint already has been added");
            }
        }

        public IWaypoint GetWaypoint(IWaypointView waypointView)
        {
            if (!_waypoints.TryGetValue(waypointView, out var waypoint))
            {
                Debug.LogError("Waypoint not found");
                return null;
            }
            
            return waypoint;
        }
    }
}