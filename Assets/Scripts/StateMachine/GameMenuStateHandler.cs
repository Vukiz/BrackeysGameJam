namespace StateMachine
{
    public class GameMenuStateHandler : IGameStateHandler
    {
        public GameState State => GameState.GameMenu;
        
        public void OnStateEnter()
        {
            // Show menu UI
            // Initialize menu components
        }

        public void OnStateExit()
        {
            // Hide menu UI
            // Cleanup menu components
        }
    }
}
