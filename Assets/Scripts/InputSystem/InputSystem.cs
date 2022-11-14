#nullable enable
using System;
using System.Collections.Generic;
using NovemberProject.CommonUIStuff;
using UniRx;
using UnityEngine;

namespace NovemberProject.InputSystem
{
    public sealed class InputSystem : InitializableBehaviour
    {
        private readonly InputHandlersFactory _inputHandlersFactory = new();
        private readonly List<InputHandler> _globalInputHandlers = new();
        private readonly Subject<Unit> _onHandleInput = new();

        private bool _isActive;

        public IObservable<Unit> OnHandleInput => _onHandleInput;

        protected override void OnInitialized()
        {
            base.OnInitialized();

            _isActive = true;
        }

        public T GetInputHandler<T>() where T : InputHandler, new() => _inputHandlersFactory.GetInputHandler<T>();

        public void AddGlobalInputHandler<T>() where T : InputHandler, new()
        {
            foreach (InputHandler globalInputHandler in _globalInputHandlers)
            {
                if (globalInputHandler.GetType() == typeof(T))
                {
                    Debug.LogError(
                        $"Trying to add global handler of type {typeof(T)}, but there is already one active");
                    return;
                }
            }

            var handler = _inputHandlersFactory.GetInputHandler<T>();
            _globalInputHandlers.Add(handler);
        }

        private void Update()
        {
            if (!_isActive)
            {
                return;
            }

            foreach (InputHandler globalInputHandler in _globalInputHandlers)
            {
                globalInputHandler.HandleInput();
            }

            _onHandleInput.OnNext(Unit.Default);
        }
    }
}