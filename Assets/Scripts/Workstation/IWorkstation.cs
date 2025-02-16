using Rails;

namespace Workstation
{
    public interface IWorkstation : IWaypoint
    {
        System.Action NoSlotsLeft { get; set; }
    }
}