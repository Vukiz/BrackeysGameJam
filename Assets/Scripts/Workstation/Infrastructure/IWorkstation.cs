using System;
using Factories.Infrastructure;
using Orders.Implementation;
using Rails.Infrastructure;
using SushiBelt.Infrastructure;
using Workstation.Views;

namespace Workstation.Infrastructure
{
    public interface IWorkstation : IWaypoint
    {
        void AddOrderToSushiBelt(Order order);
        void Cleanup();
        event Action<IRobot> RobotReachedStationWithNoEmptySlots;
        void SetView(WorkstationView workstationView, ISushiBelt sushiBelt);
    }
}