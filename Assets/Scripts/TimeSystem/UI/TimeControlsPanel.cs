#nullable enable
using NovemberProject.CommonUIStuff;
using NovemberProject.System;
using NovemberProject.System.UI;
using UniRx;
using UnityEngine;

namespace NovemberProject.TimeSystem.UI
{
    public class TimeControlsPanel : InitializableBehaviour
    {
        private const int PAUSE_BUTTON_INDEX = 0;
        private const int PLAY_BUTTON_INDEX = 1;
        private const int SPEED_UP_BUTTON_INDEX = 2;

        [SerializeField]
        private RadioButtonGroup _buttonsGroup = null!;

        protected override void Initialize()
        {
            Game.Instance.TimeSystem.Status
                .TakeUntilDisable(this)
                .Subscribe(OnTimeSystemStatusChanged);
            _buttonsGroup.OnButtonClicked
                .TakeUntilDisable(this)
                .Subscribe(OnButtonClicked);
        }

        private void OnTimeSystemStatusChanged(TimeSystemStatus status)
        {
            switch (status)
            {
                case TimeSystemStatus.Play:
                    _buttonsGroup.SetClickedButtonSilently(PLAY_BUTTON_INDEX);
                    break;
                case TimeSystemStatus.Pause:
                    _buttonsGroup.SetClickedButtonSilently(PAUSE_BUTTON_INDEX);
                    break;
                case TimeSystemStatus.SpedUp:
                    _buttonsGroup.SetClickedButtonSilently(SPEED_UP_BUTTON_INDEX);
                    break;
            }
        }

        private void OnButtonClicked(int buttonIndex)
        {
            switch (buttonIndex)
            {
                case PAUSE_BUTTON_INDEX:
                    Game.Instance.TimeSystem.PauseTime();
                    break;
                case PLAY_BUTTON_INDEX:
                    Game.Instance.TimeSystem.ResetTimeScale();
                    break;
                case SPEED_UP_BUTTON_INDEX:
                    Game.Instance.TimeSystem.SpeedUp();
                    break;
            }
        }
    }
}