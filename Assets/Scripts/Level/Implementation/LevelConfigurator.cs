using System.Collections.Generic;
using System.Linq;
using Factories.Implementation;
using Factories.Infrastructure;
using Level.Data;
using Level.Infrastructure;
using Level.Views;
using Rails.Implementation;
using Orders.Implementation;
using Orders.Infrastructure;
using Rails.Infrastructure;
using SushiBelt.Infrastructure;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;
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

        private readonly List<IWorkstation> _workstations = new();

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
            if (number < 0)
            {
                number = 0;
            }

            var levelData = _levelsHolder.Levels[Mathf.Min(number, _levelsHolder.Levels.Count - 1)];
            var levelView = _container.InstantiatePrefabForComponent<LevelView>(levelData.LevelViewPrefab);
            _collisionsTracker.Reset();
            _waypointProvider.Cleanup();
            SetupFactoryAvailabilityTracker(levelView);
            SetupOrders(levelData.Orders, levelData.RandomOrdersSettings);
            SetupWorkstations(levelView);
            SetupRailSwitches(levelView);

            _collisionsTracker.TrackCollisions();
            _orderProvider.StartProcessingOrders().Forget();

            return levelView;
        }

        private void SetupOrders(List<OrderData> levelDataOrders,
            List<RandomOrdersSettings> levelDataRandomOrdersSettings)
        {
            var orders = levelDataOrders
                .Select(orderData => new Order(orderData.RequiredWorkTypes.ToList(), orderData.TimeLimitSeconds))
                .Cast<IOrder>()
                .ToList();

            _orderProvider.Initialize(orders, levelDataRandomOrdersSettings);
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

        private void SetupWorkstations(LevelView levelView)
        {
            foreach (var workstation in _workstations)
            {
                workstation.Cleanup();
            }

            _workstations.Clear(); // Cleanup workstations and associated sushi belts
            foreach (var workstationView in levelView.WorkstationViews)
            {
                var sushiBelt = _container.Resolve<ISushiBelt>();
                sushiBelt.SetView(workstationView.SushiBeltView);
                _factoryAvailabilityTracker.RegisterSushiBeltForTracking(sushiBelt);
                _orderProvider.RegisterSushiBelt(sushiBelt); // was cleaned up in initialization
                var workstation = _container.Resolve<IWorkstation>();
                workstation.SetView(workstationView, sushiBelt);
                _workstations.Add(workstation);
                _waypointProvider.RegisterWaypoint(workstationView, workstation);
                _collisionsTracker.RegisterWorkstation(workstation);
            }
        }

        private void SetupRailSwitches(LevelView levelView)
        {
            foreach (var railSwitchView in levelView.RailSwitchViews)
            {
                var railSwitch = _container.Resolve<IRailSwitch>();
                railSwitch.SetView(railSwitchView);
                _waypointProvider.RegisterWaypoint(railSwitchView, railSwitch);
            }

            var rails = new List<(IWaypoint from, IWaypoint to)>();
            foreach (var railSwitchView in levelView.RailSwitchViews)
            {
                var railSwitch = _waypointProvider.GetWaypoint(railSwitchView);
                foreach (var waypointView in railSwitchView.NeighbourWaypoints)
                {
                    var waypoint = _waypointProvider.GetWaypoint(waypointView);
                    railSwitch.AddNeighbour(waypoint);
                    CreateRail(railSwitch, waypoint, levelView.RailPrefab, levelView.RailsParent, rails);
                }
            }
        }

        private void CreateRail(IWaypoint from, IWaypoint to, GameObject railPrefab, Transform parent,
            List<(IWaypoint from, IWaypoint to)> existingRails)
        {
            var existingPair = existingRails.FirstOrDefault(rail =>
                (rail.from == from && rail.to == to) || (rail.from == to && rail.to == from));

            if (existingPair != default)
            {
                return;
            }

            var rail = new GameObject
            {
                name = "Rail",
                transform =
                {
                    parent = parent
                }
            };

            var spline = rail.AddComponent<SplineContainer>().Spline;
            var fromPosition = new float3(from.Position.x, 0.01f, from.Position.z);
            var toPosition = new float3(to.Position.x, 0.01f, to.Position.z);
            var fromKnot = new BezierKnot(fromPosition);
            var toKnot = new BezierKnot(toPosition);
            spline.Add(fromKnot);
            spline.Insert(1, toKnot);
            existingRails.Add((from, to));
            var splineInstantiate = rail.AddComponent<SplineInstantiate>();
            splineInstantiate.itemsToInstantiate =
                new[] { new SplineInstantiate.InstantiableItem { Prefab = railPrefab } };
            splineInstantiate.UpdateInstances();
        }
    }
}