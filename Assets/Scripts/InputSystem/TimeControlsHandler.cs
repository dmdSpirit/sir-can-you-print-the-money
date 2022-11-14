#nullable enable
using NovemberProject.System;
using UnityEngine;

namespace NovemberProject.InputSystem
{
    public sealed class TimeControlsHandler : InputHandler
    {
        public override void HandleInput()
        {
            if (Input.GetKeyDown(InputKeys.PAUSE_TOGGLE_KEY))
            {
                Game.Instance.TimeSystem.TogglePause();
            }

            if (Input.GetKeyDown(InputKeys.PAUSE_KEY))
            {
                Game.Instance.TimeSystem.PauseTime();
            }

            if (Input.GetKeyDown(InputKeys.PLAY_KEY))
            {
                Game.Instance.TimeSystem.ResetTimeScale();
            }

            if (Input.GetKeyDown(InputKeys.SPEED_UP_KEY))
            {
                Game.Instance.TimeSystem.SpeedUp();
            }
        }
    }
}