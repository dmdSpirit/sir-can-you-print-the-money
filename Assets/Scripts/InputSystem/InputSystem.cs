#nullable enable
using System;
using System.Collections.Generic;
using NovemberProject.CommonUIStuff;
using UniRx;
using UnityEngine;

namespace NovemberProject.InputSystem
{
    public class InputSystem : InitializableBehaviour
    {
        public const KeyCode PAUSE_TOGGLE_KEY = KeyCode.Space;
        public const KeyCode PAUSE_KEY = KeyCode.Alpha1;
        public const KeyCode PLAY_KEY = KeyCode.Alpha2;
        public const KeyCode SPEED_UP_KEY = KeyCode.Alpha3;
        public const KeyCode CAMERA_FORWARD_KEY = KeyCode.W;
        public const KeyCode CAMERA_BACKWARD_KEY = KeyCode.S;
        public const KeyCode CAMERA_LEFT_KEY = KeyCode.A;
        public const KeyCode CAMERA_RIGHT_KEY = KeyCode.D;
        public const KeyCode CAMERA_ZOOM_IN_KEY = KeyCode.E;
        public const KeyCode CAMERA_ZOOM_OUT_KEY = KeyCode.Q;
        public const KeyCode TOGGLE_CHEAT_MENU = KeyCode.H;
        public const KeyCode TOGGLE_SYSTEM_PANEL = KeyCode.Y;

        private readonly InputHandlersFactory _inputHandlersFactory = new();
        private readonly List<InputHandler> _globalInputHandlers = new();
        private readonly Subject<Unit> _onHandleInput = new();

        private bool _isActive;

        public IObservable<Unit> OnHandleInput => _onHandleInput;

        protected override void Initialize()
        {
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