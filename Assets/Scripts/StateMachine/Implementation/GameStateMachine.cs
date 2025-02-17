using System;
using System.Collections.Generic;
using StateMachine.Data;
using StateMachine.Infrastructure;
using UnityEngine;
using Zenject;

namespace StateMachine.Implementation
{
    public class GameStateMachine : IInitializable, IDisposable, IGameStateMachine
    {
        private readonly Dictionary<GameState, IGameStateHandler> _handlers;
        private IGameStateHandler _currentHandler;
        
        public GameState CurrentState => _currentHandler?.State ?? GameState.GameStart;
        
        public event Action<GameState> StateChanged;

        public GameStateMachine(IEnumerable<IGameStateHandler> handlers)
        {
            _handlers = new Dictionary<GameState, IGameStateHandler>();
            foreach (var handler in handlers)
            {
                handler.GameStateChangeRequested += ChangeState;
                _handlers.Add(handler.State, handler);
            }
        }
        

        public void Initialize()
        {
            ChangeState(GameState.GameStart);
        }

        public void Dispose()
        {
            _currentHandler?.OnStateExit();
        }

        public void ChangeState(GameState newState)
        {
            Debug.Log($"Changing state from {CurrentState} to {newState}");
            if (_currentHandler?.State == newState)
            {
                return;
            }

            _currentHandler?.OnStateExit();
            
            if (_handlers.TryGetValue(newState, out var handler))
            {
                _currentHandler = handler;
                _currentHandler.OnStateEnter();
                StateChanged?.Invoke(newState);
            }
            else
            {
                Debug.LogError($"No handler found for state {newState}");
            }
        }
    }
}