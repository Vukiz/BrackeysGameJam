using System;
using Factories.Infrastructure;
using Rails.Infrastructure;
using SushiBelt.Infrastructure;
using Workstation.Views;

namespace Workstation.Infrastructure
{
    public interface IWorkstation : IWaypoint
    {
        event Action<IRobot> RobotReachedStationWithNoEmptySlots;
        void SetView(WorkstationView workstationView, ISushiBelt sushiBelt);
    }
}