using StateMachine.Data;
using StateMachine.Infrastructure;
using StateMachine.Views;

namespace StateMachine.Implementation
{
    public class GameStartHandler : IGameStateHandler
    {
        private readonly CanvasView _canvasView;
        private readonly IGameStateMachine _gameStateMachine;
        public GameState State => GameState.GameStart;

        public GameStartHandler(
            CanvasView canvasView,
            IGameStateMachine gameStateMachine
        )
        {
            _canvasView = canvasView;
            _gameStateMachine = gameStateMachine;
        }

        public void OnStateEnter()
        {
            _canvasView.GameStartView.StartButton.onClick.AddListener(OnStartButtonClick);
            _canvasView.GameStartView.Show();
        }

        private async void OnStartButtonClick()
        {
            _canvasView.GameStartView.StartButton.onClick.RemoveListener(OnStartButtonClick);
            await _canvasView.LoaderView.Show();
            _gameStateMachine.ChangeState(GameState.GameActive);
        }

        public void OnStateExit()
        {
            _canvasView.GameStartView.Hide();
        }
    }
}