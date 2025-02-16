using Factories.Data;
using Factories.View;
using Orders;
using UnityEngine;
using Zenject;

namespace Factories.Implementation
{
    public class RobotProvider
    {
        private readonly DiContainer _container;
        private readonly FactoriesConfiguration _factoriesConfiguration;

        public RobotProvider(DiContainer container, FactoriesConfiguration factoriesConfiguration)
        {
            _container = container;
            _factoriesConfiguration = factoriesConfiguration;
        }

        public IRobot Create(WorkType workType, Vector3 position)
        {
            var prefab = _factoriesConfiguration.GetRobotView(workType);
            var view = _container.InstantiatePrefabForComponent<RobotView>(prefab);
            view.transform.position = position;
            var model = _factoriesConfiguration.GetRobotModel(workType);
            var viewModel = _container.Resolve<Robot>();
            viewModel.Initialize(view, model);
            return viewModel;
        }
    }
}