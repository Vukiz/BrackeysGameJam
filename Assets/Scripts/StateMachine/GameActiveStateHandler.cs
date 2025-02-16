namespace StateMachine
{
    public class GameActiveStateHandler : IGameStateHandler
    {
        public GameState State => GameState.GameActive;

        public GameActiveStateHandler()
        {
        }

        public void OnStateEnter()
        {
            // SHOW Active Game UI
            // TODO Get Selected level configuration and start the game
            // Spawm Level Prefab and start sending orders until finished
        }

        public void OnStateExit()
        {
            // Hide ui
        }
    }
}