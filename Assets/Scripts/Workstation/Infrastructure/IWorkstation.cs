using Rails.Infrastructure;
using SushiBelt.Infrastructure;
using Workstation.Views;

namespace Workstation.Infrastructure
{
    public interface IWorkstation : IWaypoint
    {
        void SetView(WorkstationView workstationView, ISushiBelt sushiBelt);
    }
}