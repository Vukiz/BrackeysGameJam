using System;
using StateMachine.Data;

namespace StateMachine.Infrastructure
{
    public interface IGameStateHandler
    {
        event Action<GameState> GameStateChangeRequested; 
        void OnStateEnter();
        void OnStateExit();
        GameState State { get; }
    }
}
