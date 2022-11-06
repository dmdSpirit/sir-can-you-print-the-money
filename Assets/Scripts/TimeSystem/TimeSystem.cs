#nullable enable
using System;
using System.Collections.Generic;
using NovemberProject.System.UI;
using UniRx;
using UnityEngine;
using UnityEngine.Assertions;

namespace NovemberProject.TimeSystem
{
    public class TimeSystem : InitializableBehaviour
    {
        private const float DEFAULT_TIME_SCALE = 1f;
        private const float PAUSED_TIME_SCALE = 0f;

        private readonly CompositeDisposable _timerSubs = new();
        private readonly List<Timer> _timers = new();
        private readonly List<Timer> _unscaledTimers = new();
        private readonly ReactiveProperty<float> _timeScale = new();
        private readonly ReactiveProperty<TimeSystemStatus> _status = new();
        private readonly Subject<float> _onUpdate = new();

        private TimeSystemStatus _statusBeforePause;

        [SerializeField]
        private float _speedUpScale = 8f;

        public IReadOnlyReactiveProperty<float> TimeScale => _timeScale;
        public IReadOnlyReactiveProperty<TimeSystemStatus> Status => _status;
        public IObservable<float> OnUpdate => _onUpdate;

        protected override void Initialize()
        {
            ResetTimeScale();
        }

        private void Update()
        {
            AddProgressToTimers(_unscaledTimers.ToArray(), Time.deltaTime);
            float deltaTime = Time.deltaTime * _timeScale.Value;
            if (deltaTime == 0)
            {
                return;
            }

            AddProgressToTimers(_timers.ToArray(), deltaTime);

            _onUpdate.OnNext(deltaTime);
        }

        private void AddProgressToTimers(Timer[] timers, float deltaTime)
        {
            for (var index = 0; index < timers.Length; index++)
            {
                if (!timers[index].IsActive)
                {
                    continue;
                }

                timers[index].AddProgress(deltaTime);
            }
        }

        private void OnDestroy()
        {
            _timerSubs.Dispose();
        }

        public void ResetTimeScale()
        {
            _timeScale.Value = DEFAULT_TIME_SCALE;
            _status.Value = TimeSystemStatus.Play;
        }

        public void PauseTime()
        {
            _timeScale.Value = PAUSED_TIME_SCALE;
            _statusBeforePause = _status.Value;
            _status.Value = TimeSystemStatus.Pause;
        }

        public void SpeedUp()
        {
            _timeScale.Value = _speedUpScale;
            _status.Value = TimeSystemStatus.SpedUp;
        }

        public void TogglePause()
        {
            if (_status.Value != TimeSystemStatus.Pause)
            {
                PauseTime();
            }
            else
            {
                RestoreAfterPause();
            }
        }

        public Timer CreateTimer(float duration, Action<Timer>? callback = null)
        {
            var timer = new Timer(duration, callback);
            timer.OnTimerCanceled.Subscribe(RemoveTimer).AddTo(_timerSubs);
            timer.OnTimerFinished.Subscribe(RemoveTimer).AddTo(_timerSubs);
            _timers.Add(timer);
            return timer;
        }

        public Timer CreateUnscaledTimer(float duration)
        {
            var timer = new Timer(duration);
            timer.OnTimerCanceled.Subscribe(RemoveTimer).AddTo(_timerSubs);
            timer.OnTimerFinished.Subscribe(RemoveTimer).AddTo(_timerSubs);
            _unscaledTimers.Add(timer);
            return timer;
        }

        public float GetTimeLeft(Timer timer)
        {
            Assert.IsTrue(_timers.Contains(timer) || _unscaledTimers.Contains(timer));
            return (timer.Duration - timer.Progress) * _timeScale.Value;
        }

        private void RemoveTimer(Timer timer)
        {
            if (_timers.Contains(timer))
            {
                _timers.Remove(timer);
            }
            else if (_unscaledTimers.Contains(timer))
            {
                _unscaledTimers.Remove(timer);
            }
        }

        private void RestoreAfterPause()
        {
            if (_status.Value != TimeSystemStatus.Pause)
            {
                return;
            }

            if (_statusBeforePause == TimeSystemStatus.SpedUp)
            {
                SpeedUp();
                return;
            }

            ResetTimeScale();
        }
    }
}