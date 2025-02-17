using System;
using StateMachine.Data;
using StateMachine.Infrastructure;

namespace StateMachine.Implementation
{
    public abstract class GameStateHandlerBase : IGameStateHandler
    {
        public event Action<GameState> GameStateChangeRequested;

        public virtual void OnStateEnter()
        {
        }

        public virtual void OnStateExit()
        {
        }

        public abstract GameState State { get; }
        
        protected void RequestStateChange(GameState newState)
        {
            GameStateChangeRequested?.Invoke(newState);
        }
    }
}