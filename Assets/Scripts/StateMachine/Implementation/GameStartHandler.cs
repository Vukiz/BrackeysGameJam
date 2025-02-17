using StateMachine.Data;
using StateMachine.Infrastructure;
using StateMachine.Views;

namespace StateMachine.Implementation
{
    public class GameStartHandler : GameStateHandlerBase
    {
        private readonly CanvasView _canvasView;
        public override GameState State => GameState.GameStart;

        public GameStartHandler(
            CanvasView canvasView
        )
        {
            _canvasView = canvasView;
        }

        public override void OnStateEnter()
        {
            _canvasView.GameStartView.StartButton.onClick.AddListener(OnStartButtonClick);
            _canvasView.GameStartView.Show();
        }

        private async void OnStartButtonClick()
        {
            _canvasView.GameStartView.StartButton.onClick.RemoveListener(OnStartButtonClick);
            await _canvasView.LoaderView.Show();
            RequestStateChange(GameState.GameActive);
        }

        public override void OnStateExit()
        {
            _canvasView.GameStartView.Hide();
        }
    }
}