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
        public override GameState State => GameState.GameActive;
        
        private LevelView _levelView;

        public GameActiveStateHandler(
            CanvasView canvasView,
            ILevelConfigurator levelConfigurator,
            IOrderProvider orderProvider
        )
        {
            _canvasView = canvasView;
            _levelConfigurator = levelConfigurator;
            _orderProvider = orderProvider;
        }

        public override async void OnStateEnter()
        {
            _canvasView.GameActiveView.ExitButton.onClick.AddListener(OnExitButtonClicked);
            await _canvasView.GameActiveView.Show();
            _levelView = _levelConfigurator.SpawnLevel(0);
            await _canvasView.LoaderView.Hide();
            _orderProvider.LevelCompleted += OnLevelCompleted;
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
            _orderProvider.LevelCompleted -= OnLevelCompleted;
            _canvasView.GameActiveView.Hide();
            Object.Destroy(_levelView.gameObject);
        }
    }
}