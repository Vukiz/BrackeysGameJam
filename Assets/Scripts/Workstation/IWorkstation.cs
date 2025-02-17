using Rails;

namespace Workstation
{
    public interface IWorkstation : IWaypoint
    {
        void SetView(WorkstationView workstationView);
        event System.Action NoSlotsLeft;
    }
}