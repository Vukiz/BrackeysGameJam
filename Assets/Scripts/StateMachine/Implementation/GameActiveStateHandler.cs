using Level.Infrastructure;
using Level.Views;
using StateMachine.Data;
using StateMachine.Views;
using UnityEngine;

namespace StateMachine.Implementation
{
    public class GameActiveStateHandler : GameStateHandlerBase
    {
        private readonly CanvasView _canvasView;
        private readonly ILevelConfigurator _levelConfigurator;
        private readonly IOrderProvider _orderProvider;
        private readonly ICollisionsTracker _collisionsTracker;
        public override GameState State => GameState.GameActive;

        private LevelView _levelView;

        public GameActiveStateHandler(
            CanvasView canvasView,
            ILevelConfigurator levelConfigurator,
            IOrderProvider orderProvider,
            ICollisionsTracker collisionsTracker
        )
        {
            _canvasView = canvasView;
            _levelConfigurator = levelConfigurator;
            _orderProvider = orderProvider;
            _collisionsTracker = collisionsTracker;
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

        private void OnRobotCollisionDetected()
        {
            RequestStateChange(GameState.GameEnded);
            // TODO VFX and SFX and play explosion feedback
        }

        private void OnLevelCompleted()
        {
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