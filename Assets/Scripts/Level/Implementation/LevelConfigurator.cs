using System.Collections.Generic;
using System.Linq;
using Factories.Implementation;
using Factories.Infrastructure;
using Level.Data;
using Level.Infrastructure;
using Level.Views;
using Rails.Implementation;
using Orders;
using Rails.Infrastructure;
using SushiBelt.Infrastructure;
using Workstation.Infrastructure;
using Zenject;

namespace Level.Implementation
{
    public class LevelConfigurator : ILevelConfigurator
    {
        private readonly DiContainer _container;
        private readonly LevelsHolder _levelsHolder;
        private readonly IWaypointProvider _waypointProvider;
        private readonly IFactoryAvailabilityTracker _factoryAvailabilityTracker;
        private readonly IOrderProvider _orderProvider;
        private readonly ICollisionsTracker _collisionsTracker;

        public LevelConfigurator(
            DiContainer container,
            LevelsHolder levelsHolder,
            IWaypointProvider waypointProvider,
            IFactoryAvailabilityTracker factoryAvailabilityTracker,
            IOrderProvider orderProvider,
            ICollisionsTracker collisionsTracker
        )
        {
            _container = container;
            _levelsHolder = levelsHolder;
            _waypointProvider = waypointProvider;
            _factoryAvailabilityTracker = factoryAvailabilityTracker;
            _orderProvider = orderProvider;
            _collisionsTracker = collisionsTracker;
        }

        public LevelView SpawnLevel(int number)
        {
            var levelData = _levelsHolder.Levels[number];
            var levelView = _container.InstantiatePrefabForComponent<LevelView>(levelData.LevelViewPrefab);
            _collisionsTracker.Reset();
            SetupFactoryAvailabilityTracker(levelView);
            SetupOrders(levelData.Orders);
            SetupSushiBelts(levelView);
            SetupWorkstations(levelView);
            SetupRailSwitches(levelView);
            
            _collisionsTracker.TrackCollisions();
            _orderProvider.StartProcessingOrders().Forget();

            return levelView;
        }

        private void SetupOrders(List<OrderData> levelDataOrders)
        {
            var orders = levelDataOrders
                .Select(orderData => new Order(orderData.RequiredWorkTypes, orderData.TimeLimitSeconds))
                .Cast<IOrder>()
                .ToList();

            _orderProvider.Initialize(orders);
        }

        private void SetupFactoryAvailabilityTracker(LevelView levelView)
        {
            foreach (var factorySlotView in levelView.FactorySlots)
            {
                var factorySlot = _container.Resolve<FactorySlot>();
                factorySlot.Initialize(factorySlotView);
                _waypointProvider.RegisterWaypoint(factorySlotView, factorySlot);
                _collisionsTracker.RegisterFactorySlot(factorySlot);
            }

            _factoryAvailabilityTracker.Initialize(levelView.FactorySlots, levelView.FactoriesParent,
                levelView.RobotsParent);
        }

        private void SetupSushiBelts(LevelView levelView)
        {
            foreach (var sushiBeltView in levelView.SushiBeltViews)
            {
                var sushiBelt = _container.Resolve<ISushiBelt>();
                sushiBelt.SetView(sushiBeltView);
                _factoryAvailabilityTracker.RegisterSushiBeltForTracking(sushiBelt);
                _orderProvider.RegisterSushiBelt(sushiBelt);
            }
        }

        private void SetupWorkstations(LevelView levelView)
        {
            foreach (var workstationView in levelView.WorkstationViews)
            {
                var workstation = _container.Resolve<IWorkstation>();
                workstation.SetView(workstationView);
                _waypointProvider.RegisterWaypoint(workstationView, workstation);
            }
        }

        private void SetupRailSwitches(LevelView levelView)
        {
            foreach (var railSwitchView in levelView.RailSwitchViews)
            {
                var railSwitch = _container.Resolve<RailSwitch>();
                railSwitch.SetView(railSwitchView);
                _waypointProvider.RegisterWaypoint(railSwitchView, railSwitch);
            }

            foreach (var railSwitchView in levelView.RailSwitchViews)
            {
                var railSwitch = _waypointProvider.GetWaypoint(railSwitchView);
                foreach (var waypointView in railSwitchView.NeighbourWaypoints)
                {
                    var waypoint = _waypointProvider.GetWaypoint(waypointView);
                    railSwitch.AddNeighbour(waypoint);
                }
            }
        }
    }
}