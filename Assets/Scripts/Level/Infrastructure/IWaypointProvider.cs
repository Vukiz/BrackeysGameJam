using Rails.Infrastructure;

namespace Level.Infrastructure
{
    public interface IWaypointProvider
    {
        void Cleanup();
        void RegisterWaypoint(IWaypointView waypointView, IWaypoint waypoint);
        IWaypoint GetWaypoint(IWaypointView waypointView);
    }
}