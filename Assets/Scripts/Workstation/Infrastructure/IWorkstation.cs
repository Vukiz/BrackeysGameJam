using Rails;
using Rails.Infrastructure;
using Workstation.Views;

namespace Workstation.Infrastructure
{
    public interface IWorkstation : IWaypoint
    {
        void SetView(WorkstationView workstationView);
        event System.Action NoSlotsLeft;
    }
}