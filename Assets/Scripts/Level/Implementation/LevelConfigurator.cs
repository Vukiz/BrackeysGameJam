using Factories.Infrastructure;
using Level.Data;
using Level.Infrastructure;
using Level.Views;
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

        public LevelConfigurator(
            DiContainer container,
            LevelsHolder levelsHolder,
            IWaypointProvider waypointProvider,
            IFactoryAvailabilityTracker factoryAvailabilityTracker
        )
        {
            _container = container;
            _levelsHolder = levelsHolder;
            _waypointProvider = waypointProvider;
            _factoryAvailabilityTracker = factoryAvailabilityTracker;
        }

        public void SpawnLevel(int number)
        {
            var levelData = _levelsHolder.Levels[number];
            var levelView = _container.InstantiatePrefabForComponent<LevelView>(levelData.LevelViewPrefab);
            SetupFactoryAvailabilityTracker(levelView);
            SetupSushiBelts(levelView);
            SetupWorkstations(levelView);
            SetupRailSwitches(levelView);
        }

        private void SetupFactoryAvailabilityTracker(LevelView levelView)
        {
            _factoryAvailabilityTracker.Initialize(levelView.FactorySlots);
        }

        private void SetupSushiBelts(LevelView levelView)
        {
            foreach (var sushiBeltView in levelView.SushiBeltViews)
            {
                var sushiBelt = _container.Resolve<ISushiBelt>();
                sushiBelt.SetView(sushiBeltView);
                _factoryAvailabilityTracker.RegisterSushiBeltForTracking(sushiBelt);
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
                var railSwitch = _container.Resolve<IRailSwitch>();
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