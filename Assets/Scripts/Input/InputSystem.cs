#nullable enable
using NovemberProject.System;
using NovemberProject.System.UI;
using UnityEngine;

namespace NovemberProject.Input
{
    public class InputSystem : InitializableBehaviour
    {
        private const KeyCode PAUSE_TOGGLE_KEY = KeyCode.Space;
        private const KeyCode PAUSE_KEY = KeyCode.Alpha1;
        private const KeyCode PLAY_KEY = KeyCode.Alpha2;
        private const KeyCode SPEED_UP_KEY = KeyCode.Alpha3;

        private bool _isActive;

        protected override void Initialize()
        {
            _isActive = true;
        }

        private void Update()
        {
            if (!_isActive)
            {
                return;
            }

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