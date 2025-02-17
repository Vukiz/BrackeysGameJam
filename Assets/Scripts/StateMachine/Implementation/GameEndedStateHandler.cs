using StateMachine.Data;
using StateMachine.Infrastructure;
using StateMachine.Views;

namespace StateMachine.Implementation
{
    public class GameEndedStateHandler : IGameStateHandler
    {
        private readonly CanvasView _canvasView;
        private readonly IGameStateMachine _gameStateMachine;
        public GameState State => GameState.GameEnded;

        public GameEndedStateHandler(
            CanvasView canvasView,
            IGameStateMachine gameStateMachine
        )
        {
            _canvasView = canvasView;
            _gameStateMachine = gameStateMachine;
        }

        public async void OnStateEnter()
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
            _gameStateMachine.ChangeState(GameState.GameThanksForPlaying);
        }

        public void OnStateExit()
        {
            _canvasView.GameEndView.Hide();
        }
    }
}