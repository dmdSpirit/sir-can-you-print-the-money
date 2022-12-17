#nullable enable
using NovemberProject.System;

namespace NovemberProject.Input
{
    public sealed class TimeControlsHandler : InputHandler
    {
        public override void HandleInput()
        {
            if (UnityEngine.Input.GetKeyDown(InputKeys.PAUSE_TOGGLE_KEY))
            {
                Game.Instance.TimeSystem.TogglePause();
            }

            if (UnityEngine.Input.GetKeyDown(InputKeys.PAUSE_KEY))
            {
                Game.Instance.TimeSystem.PauseTime();
            }

            if (UnityEngine.Input.GetKeyDown(InputKeys.PLAY_KEY))
            {
                Game.Instance.TimeSystem.ResetTimeScale();
            }

            if (UnityEngine.Input.GetKeyDown(InputKeys.SPEED_UP_KEY))
            {
                Game.Instance.TimeSystem.SpeedUp();
            }
        }
    }
}