#nullable enable
using System;
using NovemberProject.CommonUIStuff;
using NovemberProject.System;
using UniRx;
using UnityEngine;

namespace NovemberProject.Input
{
    public class InputSystem : InitializableBehaviour
    {
        private const KeyCode PAUSE_TOGGLE_KEY = KeyCode.Space;
        private const KeyCode PAUSE_KEY = KeyCode.Alpha1;
        private const KeyCode PLAY_KEY = KeyCode.Alpha2;
        private const KeyCode SPEED_UP_KEY = KeyCode.Alpha3;

        private readonly InputHandlersFactory _inputHandlersFactory = new InputHandlersFactory();
        private readonly Subject<Unit> _onHandleInput = new();

        private bool _isActive;

        public IObservable<Unit> OnHandleInput => _onHandleInput;

        protected override void Initialize()
        {
            _isActive = true;
        }

        public T GetInputHandler<T>() where T : InputHandler, new() => _inputHandlersFactory.GetInputHandler<T>();

        private void Update()
        {
            if (!_isActive)
            {
                return;
            }

            HandleInput();

            _onHandleInput.OnNext(Unit.Default);
        }

        // TODO (STAS): Move this to input handlers
        private static void HandleInput()
        {
            if (UnityEngine.Input.GetKeyDown(PAUSE_TOGGLE_KEY))
            {
                Game.Instance.TimeSystem.TogglePause();
            }

            if (UnityEngine.Input.GetKeyDown(PAUSE_KEY))
            {
                Game.Instance.TimeSystem.PauseTime();
            }

            if (UnityEngine.Input.GetKeyDown(PLAY_KEY))
            {
                Game.Instance.TimeSystem.ResetTimeScale();
            }

            if (UnityEngine.Input.GetKeyDown(SPEED_UP_KEY))
            {
                Game.Instance.TimeSystem.SpeedUp();
            }
        }
    }
}