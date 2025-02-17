using Factories.Data;
using Factories.Infrastructure;
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
            var data = _factoriesConfiguration.GetRobotData(workType);
            var robot = _container.Resolve<Robot>(); // TODO: Maybe create manually if DI is not required
            robot.Initialize(view, data);
            return robot;
        }
    }
}