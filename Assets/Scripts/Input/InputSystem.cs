#nullable enable
using NovemberProject.System;
using UnityEngine;

namespace NovemberProject.Input
{
    public class InputSystem : MonoBehaviour
    {
        private const KeyCode PAUSE_TOGGLE_KEY = KeyCode.Space;

        private bool _isActive;

        public void Initialize()
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
        }
    }
}