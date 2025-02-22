using Factories.Data;
using Factories.Views;
using Level.Infrastructure;
using Orders.Data;
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

        public FactoryProvider(
            DiContainer container,
            FactoriesConfiguration factoriesConfiguration
        )
        {
            _container = container;
            _factoriesConfiguration = factoriesConfiguration;
        }

        public IFactory Create(WorkType workType, Vector3 position, IWaypoint nextWaypoint, Transform factoryParent,
            Transform robotsParent)
        {
            var prefab = _factoriesConfiguration.GetFactoryView(workType);
            var view = _container.InstantiatePrefabForComponent<FactoryView>(prefab, factoryParent);
            view.transform.position = position;
            var data = _factoriesConfiguration.GetFactoryData(workType);
            var factory = _container.Resolve<IFactory>();
            factory.Initialize(view, data, nextWaypoint, robotsParent);
            return factory;
        }
    }
}