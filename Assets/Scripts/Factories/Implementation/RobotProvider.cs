using Factories.Data;
using Factories.Infrastructure;
using Factories.View;
using Level.Infrastructure;
using Orders;
using Orders.Data;
using UnityEngine;
using Zenject;

namespace Factories.Implementation
{
    public class RobotProvider
    {
        private readonly DiContainer _container;
        private readonly FactoriesConfiguration _factoriesConfiguration;
        private readonly ICollisionsTracker _collisionsTracker;

        public RobotProvider(
            DiContainer container,
            FactoriesConfiguration factoriesConfiguration,
            ICollisionsTracker collisionsTracker)
        {
            _container = container;
            _factoriesConfiguration = factoriesConfiguration;
            _collisionsTracker = collisionsTracker;
        }

        public IRobot Create(WorkType workType, Vector3 position, Transform parent)
        {
            var prefab = _factoriesConfiguration.GetRobotView(workType);
            var view = _container.InstantiatePrefabForComponent<RobotView>(prefab, parent);
            view.transform.position = position;
            var data = _factoriesConfiguration.GetRobotData(workType);
            var robot = _container.Resolve<Robot>(); // TODO: Maybe create manually if DI is not required
            robot.Initialize(view, data);
            robot.RobotDestroyRequested += OnRobotDestroyRequested;
            _collisionsTracker.RegisterRobot(robot);
            return robot;
        }

        private void OnRobotDestroyRequested(IRobot robot)
        {
            robot.RobotDestroyRequested -= OnRobotDestroyRequested;
            robot.Destroy();
            _collisionsTracker.RemoveRobot(robot);
        }
    }
}