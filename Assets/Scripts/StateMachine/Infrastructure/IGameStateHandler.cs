using StateMachine.Data;

namespace StateMachine.Infrastructure
{
    public interface IGameStateHandler
    {
        void OnStateEnter();
        void OnStateExit();
        GameState State { get; }
    }
}
