#nullable enable
using NovemberProject.System;
using NovemberProject.Time;
using Zenject;

namespace NovemberProject.Input
{
    public sealed class TimeControlsHandler : InputHandler
    {
        // TODO (Stas): Hack. I just don't want to deal with this right now.
        private TimeSystem _timeSystem;

        public TimeControlsHandler(TimeSystem timeSystem)
        {
            _timeSystem = timeSystem;
        }

        public override void HandleInput()
        {
            if (UnityEngine.Input.GetKeyDown(InputKeys.PAUSE_TOGGLE_KEY))
            {
                _timeSystem.TogglePause();
            }

            if (UnityEngine.Input.GetKeyDown(InputKeys.PAUSE_KEY))
            {
                _timeSystem.PauseTime();
            }

            if (UnityEngine.Input.GetKeyDown(InputKeys.PLAY_KEY))
            {
                _timeSystem.ResetTimeScale();
            }

            if (UnityEngine.Input.GetKeyDown(InputKeys.SPEED_UP_KEY))
            {
                _timeSystem.SpeedUp();
            }
        }
    }
}