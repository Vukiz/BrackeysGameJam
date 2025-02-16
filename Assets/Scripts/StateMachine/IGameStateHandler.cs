namespace StateMachine
{
    public interface IGameStateHandler
    {
        void OnStateEnter();
        void OnStateExit();
        GameState State { get; }
    }
}
