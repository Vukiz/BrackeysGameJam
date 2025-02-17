using Factories.Data;
using Factories.View;
using Level;
using Level.Implementation;
using Orders;
using Rails;
using Rails.Infrastructure;
using UnityEngine;
using Zenject;
using IFactory = Factories.Infrastructure.IFactory;

namespace Factories.Implementation
{
    public class FactoryProvider
    {
        private readonly DiContainer _container;
        private readonly FactoriesConfiguration _factoriesConfiguration;
        private readonly WaypointProvider _waypointProvider;

        public FactoryProvider(DiContainer container, FactoriesConfiguration factoriesConfiguration,
            WaypointProvider waypointProvider)
        {
            _container = container;
            _factoriesConfiguration = factoriesConfiguration;
            _waypointProvider = waypointProvider;
        }

        public IFactory Create(WorkType workType, Vector3 position, IWaypointView nextWaypointView)
        {
            var prefab = _factoriesConfiguration.GetFactoryView(workType);
            var view = _container.InstantiatePrefabForComponent<FactoryView>(prefab);
            view.transform.position = position;
            var data = _factoriesConfiguration.GetFactoryData(workType);
            var nextWaypoint = _waypointProvider.GetWaypoint(nextWaypointView);
            var factory = _container.Resolve<Factory>(); // TODO: Maybe create manually if DI is not required
            factory.Initialize(view, data, nextWaypoint);
            _waypointProvider.RegisterWaypoint(view, factory);
            return factory;
        }
    }
}