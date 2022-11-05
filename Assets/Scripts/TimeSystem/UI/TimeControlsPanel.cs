#nullable enable
using NovemberProject.System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace NovemberProject.TimeSystem.UI
{
    public class TimeControlsPanel : MonoBehaviour
    {
        [SerializeField]
        private Toggle _pauseButton = null!;

        [SerializeField]
        private Toggle _playButton = null!;

        [SerializeField]
        private Toggle _speedUpButton = null!;

        private void OnEnable()
        {
            Game.Instance.OnInitialized.TakeUntilDisable(this).Subscribe(_ => Initialize());
        }

        private void Initialize()
        {
            Game.Instance.TimeSystem.Status
                .TakeUntilDisable(this)
                .Subscribe(OnTimeSystemStatusChanged);
            _pauseButton.OnValueChangedAsObservable()
                .TakeUntilDisable(this)
                .Where(value => value)
                .Subscribe(PauseButtonHandler);
            _playButton.OnValueChangedAsObservable()
                .TakeUntilDisable(this)
                .Where(value => value)
                .Subscribe(PlayButtonHandler);
            _speedUpButton.OnValueChangedAsObservable()
                .TakeUntilDisable(this)
                .Where(value => value)
                .Subscribe(SpeedUpButtonHandler);
        }

        private void OnTimeSystemStatusChanged(TimeSystemStatus status)
        {
            switch (status)
            {
                case TimeSystemStatus.Play:
                    TurnToggleOn(_playButton);
                    break;
                case TimeSystemStatus.Pause:
                    TurnToggleOn(_pauseButton);
                    break;
                case TimeSystemStatus.SpedUp:
                    TurnToggleOn(_speedUpButton);
                    break;
            }
        }

        private void TurnToggleOn(Toggle toggle)
        {
            if (toggle.isOn)
            {
                return;
            }

            toggle.isOn = true;
        }

        private void PauseButtonHandler(bool _)
        {
            Game.Instance.TimeSystem.PauseTime();
        }

        private void PlayButtonHandler(bool _)
        {
            Game.Instance.TimeSystem.ResetTimeScale();
        }

        private void SpeedUpButtonHandler(bool _)
        {
            Game.Instance.TimeSystem.SpeedUp();
        }
    }
}