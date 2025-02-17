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
            var canvasViewGameStartView = _canvasView.GameStartView;
            canvasViewGameStartView.gameObject.SetActive(true);
            canvasViewGameStartView.StartButton.onClick.AddListener(OnStartButtonClick);
            canvasViewGameStartView.Show();
        }

        private async void OnStartButtonClick()
        {
            _canvasView.GameStartView.StartButton.onClick.RemoveListener(OnStartButtonClick);
            await _canvasView.LoaderView.Show();
            RequestStateChange(GameState.GameActive);
        }

        public override void OnStateExit()
        {
            var canvasViewGameStartView = _canvasView.GameStartView;
            canvasViewGameStartView.Hide();
            canvasViewGameStartView.gameObject.SetActive(false);
        }
    }
}