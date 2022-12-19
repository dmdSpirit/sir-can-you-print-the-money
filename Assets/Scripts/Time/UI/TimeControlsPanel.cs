#nullable enable
using System;
using NovemberProject.CommonUIStuff;
using NovemberProject.System;
using UniRx;
using UnityEngine;
using Zenject;

namespace NovemberProject.Time.UI
{
    public sealed class TimeControlsPanel : UIElement<object?>
    {
        private const int PAUSE_BUTTON_INDEX = 0;
        private const int PLAY_BUTTON_INDEX = 1;
        private const int SPEED_UP_BUTTON_INDEX = 2;

        private TimeSystem _timeSystem = null!;
        private bool _isLocked;
        private IDisposable? _timeStatusSub;

        [SerializeField]
        private RadioButtonGroup _buttonsGroup = null!;

        [Inject]
        private void Construct(TimeSystem timeSystem)
        {
            _timeSystem = timeSystem;
        }

        private void Start()
        {
            _buttonsGroup.OnButtonClicked
                .Subscribe(OnButtonClicked);
        }

        protected override void OnShow(object? value)
        {
            _timeStatusSub?.Dispose();
            _timeStatusSub = _timeSystem.Status
                .Subscribe(OnTimeSystemStatusChanged);
        }

        protected override void OnHide()
        {
            _timeStatusSub?.Dispose();
        }

        public void Lock()
        {
            _isLocked = true;
            _buttonsGroup.Lock();
        }

        public void Unlock()
        {
            _isLocked = false;
            _buttonsGroup.Unlock();
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
            if (_isLocked)
            {
                return;
            }

            switch (buttonIndex)
            {
                case PAUSE_BUTTON_INDEX:
                    _timeSystem.PauseTime();
                    break;
                case PLAY_BUTTON_INDEX:
                    _timeSystem.ResetTimeScale();
                    break;
                case SPEED_UP_BUTTON_INDEX:
                    _timeSystem.SpeedUp();
                    break;
            }
        }
    }
}