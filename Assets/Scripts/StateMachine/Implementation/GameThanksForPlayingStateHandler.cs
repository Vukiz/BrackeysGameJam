using Cysharp.Threading.Tasks;
using StateMachine.Data;
using StateMachine.Views;

namespace StateMachine.Implementation
{
    public class GameThanksForPlayingStateHandler : GameStateHandlerBase
    {
        private readonly CanvasView _canvasView;
        public override GameState State => GameState.GameThanksForPlaying;

        public GameThanksForPlayingStateHandler(
            CanvasView canvasView
        )
        {
            _canvasView = canvasView;
        }

        public override async void OnStateEnter()
        {
            await _canvasView.ThanksForPlayingView.Show();
            await UniTask.Delay(3000);
            _canvasView.ThanksForPlayingView.ExitButton.onClick.AddListener(OnExitButtonClicked);
        }

        private void OnExitButtonClicked()
        {
            _canvasView.ThanksForPlayingView.ExitButton.onClick.RemoveListener(OnExitButtonClicked);
            RequestStateChange(GameState.GameStart);
        }

        public override async void OnStateExit()
        {
            await _canvasView.ThanksForPlayingView.Hide();
        }
    }
}