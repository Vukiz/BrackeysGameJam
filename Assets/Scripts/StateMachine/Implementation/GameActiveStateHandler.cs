using System;
using Cysharp.Threading.Tasks;
using Factories.Infrastructure;
using Level.Infrastructure;
using Level.Views;
using StateMachine.Data;
using StateMachine.Views;
using VFX.Data;
using VFX.Infrastructure;
using Object = UnityEngine.Object;

namespace StateMachine.Implementation
{
    public class GameActiveStateHandler : GameStateHandlerBase
    {
        private readonly CanvasView _canvasView;
        private readonly ILevelConfigurator _levelConfigurator;
        private readonly IOrderProvider _orderProvider;
        private readonly ICollisionsTracker _collisionsTracker;
        private readonly IVFXManager _vfxManager;
        private readonly IFactoryAvailabilityTracker _factoryAvailabilityTracker;
        public override GameState State => GameState.GameActive;

        private LevelView _levelView;

        public GameActiveStateHandler(
            CanvasView canvasView,
            ILevelConfigurator levelConfigurator,
            IOrderProvider orderProvider,
            ICollisionsTracker collisionsTracker,
            IVFXManager vfxManager,
            IFactoryAvailabilityTracker factoryAvailabilityTracker
        )
        {
            _canvasView = canvasView;
            _levelConfigurator = levelConfigurator;
            _orderProvider = orderProvider;
            _collisionsTracker = collisionsTracker;
            _vfxManager = vfxManager;
            _factoryAvailabilityTracker = factoryAvailabilityTracker;
        }

        public override async void OnStateEnter()
        {
            _canvasView.GameActiveView.ExitButton.onClick.AddListener(OnExitButtonClicked);
            await _canvasView.GameActiveView.Show();
            _levelView = _levelConfigurator.SpawnLevel(0);
            await _canvasView.LoaderView.Hide();
            _orderProvider.LevelCompleted += OnLevelCompleted;
            _collisionsTracker.RobotCollisionDetected += OnRobotCollisionDetected;
        }

        private async void OnRobotCollisionDetected(IRobot robot)
        {
            _factoryAvailabilityTracker.IsPaused = true;
            _vfxManager.SpawnVFX(VFXType.Explosion, robot.Position);
            _orderProvider.Reset();
            await UniTask.Delay(TimeSpan.FromSeconds(1f));
            RequestStateChange(GameState.GameEnded);
        }

        private void OnLevelCompleted()
        {
            _collisionsTracker.Reset();
            RequestStateChange(GameState.GameEnded);
        }

        private void OnExitButtonClicked()
        {
            _canvasView.GameActiveView.ExitButton.onClick.RemoveListener(OnExitButtonClicked);
            RequestStateChange(GameState.GameStart);
        }

        public override void OnStateExit()
        {
            Unsubscribe();
            _canvasView.GameActiveView.Hide();
            Object.Destroy(_levelView.gameObject);
        }

        private void Unsubscribe()
        {
            _orderProvider.LevelCompleted -= OnLevelCompleted;
            _collisionsTracker.RobotCollisionDetected -= OnRobotCollisionDetected;
        }
    }
}