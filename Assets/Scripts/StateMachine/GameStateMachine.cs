using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace StateMachine
{
    public class GameStateMachine : IInitializable, IDisposable, IGameStateMachine
    {
        private readonly Dictionary<GameState, IGameStateHandler> _handlers;
        private IGameStateHandler _currentHandler;
        
        public GameState CurrentState => _currentHandler?.State ?? GameState.GameMenu;
        
        public event Action<GameState> StateChanged;

        public GameStateMachine(IEnumerable<IGameStateHandler> handlers)
        {
            _handlers = new Dictionary<GameState, IGameStateHandler>();
            foreach (var handler in handlers)
            {
                _handlers.Add(handler.State, handler);
            }
        }

        public void Initialize()
        {
            ChangeState(GameState.GameMenu);
        }

        public void Dispose()
        {
            _currentHandler?.OnStateExit();
        }

        public void ChangeState(GameState newState)
        {
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