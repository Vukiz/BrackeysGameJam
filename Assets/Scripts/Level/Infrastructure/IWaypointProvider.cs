using Rails.Infrastructure;

namespace Level.Infrastructure
{
    public interface IWaypointProvider
    {
        void RegisterWaypoint(IWaypointView waypointView, IWaypoint waypoint);
        IWaypoint GetWaypoint(IWaypointView waypointView);
    }
}