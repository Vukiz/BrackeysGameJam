using Factories;
using Rails.Implementation;
using Zenject;

namespace Level
{
    public class LevelConfigurator
    {
        private readonly DiContainer _container;
        private readonly LevelsHolder _levelsHolder;
        private readonly WaypointProvider _waypointProvider;
        private readonly IFactoryAvailabilityTracker _factoryAvailabilityTracker;

        public LevelConfigurator(DiContainer container, LevelsHolder levelsHolder, WaypointProvider waypointProvider,
            IFactoryAvailabilityTracker factoryAvailabilityTracker)
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
                var sushiBelt = _container.Resolve<SushiBelt.SushiBelt>(); // TODO: Maybe create manually if DI is not required
                sushiBelt.SetView(sushiBeltView);
                _factoryAvailabilityTracker.AddSushiBelt(sushiBelt);
            }
        }

        private void SetupWorkstations(LevelView levelView)
        {
            foreach (var workstationView in levelView.WorkstationViews)
            {
                var workstation = _container.Resolve<Workstation.Workstation>(); // TODO: Maybe create manually if DI is not required
                workstation.SetView(workstationView);
                _waypointProvider.RegisterWaypoint(workstationView, workstation);
            }
        }
        
        private void SetupRailSwitches(LevelView levelView)
        {
            foreach (var railSwitchView in levelView.RailSwitchViews)
            {
                var railSwitch = _container.Resolve<RailSwitch>(); // TODO: Maybe create manually if DI is not required
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