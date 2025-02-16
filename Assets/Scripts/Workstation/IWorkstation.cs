using Rails;

namespace Workstation
{
    public interface IWorkstation : IWaypoint
    {
        event System.Action NoSlotsLeft;
    }
}