using Factories;
using Rails;
using Zenject;

namespace Level
{
    public class LevelConfigurator
    {
        private readonly DiContainer _container;
        private readonly LevelsHolder _levelsHolder;
        private readonly WaypointProvider _waypointProvider;
        private readonly IFactoryAvailabilityTracker _factoryAvailabilityTracker;

        public LevelConfigurator(
            DiContainer container,
            LevelsHolder levelsHolder,
            WaypointProvider waypointProvider,
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
            var prefab = _levelsHolder.Levels[number];
            var levelView = _container.InstantiatePrefabForComponent<LevelView>(prefab);
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
                var sushiBelt = _container.Resolve<SushiBelt.ISushiBelt>();
                sushiBelt.SetView(sushiBeltView);
                _factoryAvailabilityTracker.RegisterSushiBeltForTracking(sushiBelt);
            }
        }

        private void SetupWorkstations(LevelView levelView)
        {
            foreach (var workstationView in levelView.WorkstationViews)
            {
                var workstation = _container.Resolve<Workstation.IWorkstation>();
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