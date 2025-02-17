using StateMachine.Data;
using StateMachine.Infrastructure;
using StateMachine.Views;

namespace StateMachine.Implementation
{
    public class GameEndedStateHandler : GameStateHandlerBase
    {
        private readonly CanvasView _canvasView;
        public override GameState State => GameState.GameEnded;

        public GameEndedStateHandler(
            CanvasView canvasView
        )
        {
            _canvasView = canvasView;
        }

        public override async void OnStateEnter()
        {
            // TODO If game is lost  setup for a restart or exit
            // TODO if game is won setup for next level or exit
            // TODO if level was last show ThanksForPlaying

            await _canvasView.GameEndView.Show();
            _canvasView.GameEndView.NextLevelButton.onClick.AddListener(OnNextLevelButtonClicked);
            // TODO Show a screen with Stars rating and a button to go back to menu OR next - that will send player to GameThanksForPlaying or Next Level
        }

        private void OnNextLevelButtonClicked()
        {
            _canvasView.GameEndView.NextLevelButton.onClick.RemoveListener(OnNextLevelButtonClicked);
            RequestStateChange(GameState.GameActive);
        }

        public override void OnStateExit()
        {
            _canvasView.GameEndView.Hide();
        }
    }
}