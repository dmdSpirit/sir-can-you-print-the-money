#nullable enable
using NovemberProject.System;
using UnityEngine;

namespace NovemberProject.InputSystem
{
    public sealed class TimeControlsHandler : InputHandler
    {
        public override void HandleInput()
        {
            if (Input.GetKeyDown(InputSystem.PAUSE_TOGGLE_KEY))
            {
                Game.Instance.TimeSystem.TogglePause();
            }

            if (Input.GetKeyDown(InputSystem.PAUSE_KEY))
            {
                Game.Instance.TimeSystem.PauseTime();
            }

            if (Input.GetKeyDown(InputSystem.PLAY_KEY))
            {
                Game.Instance.TimeSystem.ResetTimeScale();
            }

            if (Input.GetKeyDown(InputSystem.SPEED_UP_KEY))
            {
                Game.Instance.TimeSystem.SpeedUp();
            }
        }
    }
}