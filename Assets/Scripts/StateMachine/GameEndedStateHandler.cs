namespace StateMachine
{
    public class GameEndedStateHandler : IGameStateHandler
    {
        public GameState State => GameState.GameEnded;

        public GameEndedStateHandler()
        {
        }

        public void OnStateEnter()
        {
            // TODO Show a screen with Stars rating and a button to go back to menu OR next - that will send player to GameThanksForPlaying or Next Level
        }

        public void OnStateExit()
        {
        }
    }
}