using Level;
using Level.Infrastructure;
using StateMachine.Data;
using StateMachine.Infrastructure;
using StateMachine.Views;

namespace StateMachine.Implementation
{
    public class GameActiveStateHandler : IGameStateHandler
    {
        private readonly CanvasView _canvasView;
        private readonly ILevelConfigurator _levelConfigurator;
        private readonly IGameStateMachine _gameStateMachine;
        public GameState State => GameState.GameActive;

        public GameActiveStateHandler(
            CanvasView canvasView,
            ILevelConfigurator levelConfigurator,
            IGameStateMachine gameStateMachine
        )
        {
            _canvasView = canvasView;
            _levelConfigurator = levelConfigurator;
            _gameStateMachine = gameStateMachine;
        }

        public async void OnStateEnter()
        {
            _canvasView.GameActiveView.ExitButton.onClick.AddListener(OnExitButtonClicked);
            await _canvasView.GameActiveView.Show();
            _levelConfigurator.SpawnLevel(0);
            await _canvasView.LoaderView.Hide();
            // TODO Spawm Level Prefab and start sending orders until finished
        }

        private void OnExitButtonClicked()
        {
            _canvasView.GameActiveView.ExitButton.onClick.RemoveListener(OnExitButtonClicked);
            _gameStateMachine.ChangeState(GameState.GameStart);
        }

        public void OnStateExit()
        {
            _canvasView.GameActiveView.Hide();
        }
    }
}