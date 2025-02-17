using StateMachine.Data;

namespace StateMachine.Infrastructure
{
    public interface IGameStateMachine
    {
        void ChangeState(GameState newState);
    }
}