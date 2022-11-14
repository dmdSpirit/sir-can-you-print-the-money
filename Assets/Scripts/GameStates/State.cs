#nullable enable
using System;
using System.Collections.Generic;
using NovemberProject.InputSystem;
using UniRx;

namespace NovemberProject.GameStates
{
    public abstract class State
    {
        private readonly Subject<State> _onStateEnter = new();
        private readonly Subject<State> _onStateExit = new();
        private readonly List<InputHandler> _inputHandlers = new();

        public IObservable<State> OnStateEnter => _onStateEnter;
        public IObservable<State> OnStateExit => _onStateExit;

        public void Enter()
        {
            OnEnter();
            _onStateEnter.OnNext(this);
        }

        public void Exit()
        {
            OnExit();
            _onStateExit.OnNext(this);
        }

        public void AddInputHandler(InputHandler inputHandler)
        {
            _inputHandlers.Add(inputHandler);
        }

        public void HandleInput()
        {
            foreach (InputHandler inputHandler in _inputHandlers)
            {
                inputHandler.HandleInput();
            }
        }

        protected abstract void OnEnter();
        protected abstract void OnExit();
    }
}