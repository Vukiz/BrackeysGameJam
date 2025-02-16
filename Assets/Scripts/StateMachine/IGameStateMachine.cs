namespace StateMachine
{
    public interface IGameStateMachine
    {
        void ChangeState(GameState newState);
    }
}