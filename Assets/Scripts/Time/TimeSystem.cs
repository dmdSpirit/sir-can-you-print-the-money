#nullable enable
using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine.Assertions;

namespace NovemberProject.Time
{
    public sealed class TimeSystem
    {
        private const float DEFAULT_TIME_SCALE = 1f;
        private const float PAUSED_TIME_SCALE = 0f;

        private readonly CompositeDisposable _timerSubs = new();
        private readonly List<Timer> _timers = new();
        private readonly List<Timer> _unscaledTimers = new();
        private readonly ReactiveProperty<float> _timeScale = new();
        private readonly ReactiveProperty<TimeSystemStatus> _status = new();
        private readonly Subject<float> _onUpdate = new();

        private readonly TimeSystemSettings _settings;

        private TimeSystemStatus _statusBeforePause;

        public IReadOnlyReactiveProperty<float> TimeScale => _timeScale;
        public IReadOnlyReactiveProperty<TimeSystemStatus> Status => _status;
        public IObservable<float> OnUpdate => _onUpdate;

        public TimeSystem(TimeSystemSettings timeSystemSettings)
        {
            _settings = timeSystemSettings;
            PauseTime();
        }

        public void Update(float deltaTime)
        {
            AddProgressToTimers(_unscaledTimers.ToArray(), deltaTime);
            deltaTime *= _timeScale.Value;
            if (deltaTime == 0)
            {
                return;
            }

            AddProgressToTimers(_timers.ToArray(), deltaTime);

            _onUpdate.OnNext(deltaTime);
        }

        public void ResetTimers()
        {
            var timers = _timers.ToArray();
            for (var index = 0; index < timers.Length; index++)
            {
                Timer timer = timers[index];
                timer.Cancel();
            }

            _timers.Clear();

            var unscaledTimers = _unscaledTimers.ToArray();
            for (var index = 0; index < unscaledTimers.Length; index++)
            {
                Timer unscaledTimer = _unscaledTimers[index];
                unscaledTimer.Cancel();
            }

            _unscaledTimers.Clear();
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
            _timeScale.Value = _settings.SpeedUpScale;
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

        public void RestoreAfterPause()
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

        public Timer CreateTimer(float duration, Action<Timer>? callback = null)
        {
            var timer = new Timer(duration, callback);
            timer.OnTimerCanceled.Subscribe(RemoveTimer).AddTo(_timerSubs);
            timer.OnTimerFinished.Subscribe(RemoveTimer).AddTo(_timerSubs);
            _timers.Add(timer);
            return timer;
        }

        public Timer CreateUnscaledTimer(float duration, Action<Timer>? callback = null)
        {
            var timer = new Timer(duration, callback);
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

        public int EstimateSecondsLeft(IReadOnlyTimer timer)
        {
            float unscaledTimeLeft = timer.Duration - timer.Progress;
            if (_unscaledTimers.Contains((Timer)timer))
            {
                return (int)unscaledTimeLeft;
            }

            if (_timeScale.Value == 0)
            {
                return (int)unscaledTimeLeft;
            }

            return (int)(unscaledTimeLeft / _timeScale.Value);
        }

        public int EstimateSecondsLeftUnscaled(IReadOnlyTimer timer)
        {
            float unscaledTimeLeft = timer.Duration - timer.Progress;
            return (int)unscaledTimeLeft;
        }

        private void AddProgressToTimers(IEnumerable<Timer> timers, float deltaTime)
        {
            foreach (Timer timer in timers)
            {
                if (!timer.IsActive)
                {
                    continue;
                }

                timer.AddProgress(deltaTime);
            }
        }

        private void RemoveTimer(IReadOnlyTimer readOnlyTimer)
        {
            var timer = (Timer)readOnlyTimer;
            if (_timers.Contains(timer))
            {
                _timers.Remove(timer);
            }
            else if (_unscaledTimers.Contains(timer))
            {
                _unscaledTimers.Remove(timer);
            }
        }
    }
}