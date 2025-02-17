using Level.Infrastructure;
using StateMachine.Data;
using StateMachine.Views;

namespace StateMachine.Implementation
{
    public class GameActiveStateHandler : GameStateHandlerBase
    {
        private readonly CanvasView _canvasView;
        private readonly ILevelConfigurator _levelConfigurator;
        public override GameState State => GameState.GameActive;

        public GameActiveStateHandler(
            CanvasView canvasView,
            ILevelConfigurator levelConfigurator
        )
        {
            _canvasView = canvasView;
            _levelConfigurator = levelConfigurator;
        }

        public override async void OnStateEnter()
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
            RequestStateChange(GameState.GameStart);
        }

        public override void OnStateExit()
        {
            _canvasView.GameActiveView.Hide();
        }
    }
}