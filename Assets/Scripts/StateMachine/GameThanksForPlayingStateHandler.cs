namespace StateMachine
{
    public class GameThanksForPlayingStateHandler : IGameStateHandler
    {
        public GameState State => GameState.GameThanksForPlaying;

        public GameThanksForPlayingStateHandler()
        {
        }

        public void OnStateEnter()
        {
            // TODO Show a screen that will send player back to menu on click
        }

        public void OnStateExit()
        {
        }
    }
}