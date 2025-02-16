using Factories.Data;
using Factories.View;
using Level;
using Orders;
using Rails;
using UnityEngine;
using Zenject;

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
            var model = _factoriesConfiguration.GetFactoryModel(workType);
            var nextWaypoint = _waypointProvider.GetWaypoint(nextWaypointView);
            var viewModel = _container.Resolve<Factory>();
            viewModel.Initialize(view, model, nextWaypoint);
            _waypointProvider.RegisterWaypoint(view, viewModel);
            return viewModel;
        }
    }
}