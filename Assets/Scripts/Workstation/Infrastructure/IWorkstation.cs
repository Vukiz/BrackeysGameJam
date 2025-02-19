using System;
using Rails.Infrastructure;
using SushiBelt.Infrastructure;
using Workstation.Views;

namespace Workstation.Infrastructure
{
    public interface IWorkstation : IWaypoint
    {
        event Action NoSlotsLeft;
        void SetView(WorkstationView workstationView, ISushiBelt sushiBelt);
    }
}