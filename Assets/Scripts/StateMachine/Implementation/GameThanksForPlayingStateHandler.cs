using Cysharp.Threading.Tasks;
using StateMachine.Data;
using StateMachine.Infrastructure;
using StateMachine.Views;

namespace StateMachine.Implementation
{
    public class GameThanksForPlayingStateHandler : IGameStateHandler
    {
        private readonly CanvasView _canvasView;
        private readonly IGameStateMachine _gameStateMachine;
        public GameState State => GameState.GameThanksForPlaying;

        public GameThanksForPlayingStateHandler(
            CanvasView canvasView,
            IGameStateMachine gameStateMachine
        )
        {
            _canvasView = canvasView;
            _gameStateMachine = gameStateMachine;
        }

        public async void OnStateEnter()
        {
            await _canvasView.ThanksForPlayingView.Show();
            await UniTask.Delay(3000);
            _canvasView.ThanksForPlayingView.ExitButton.onClick.AddListener(OnExitButtonClicked);
        }

        private void OnExitButtonClicked()
        {
            _canvasView.ThanksForPlayingView.ExitButton.onClick.RemoveListener(OnExitButtonClicked);
            _gameStateMachine.ChangeState(GameState.GameStart);
        }

        public async void OnStateExit()
        {
            await _canvasView.ThanksForPlayingView.Hide();
        }
    }
}