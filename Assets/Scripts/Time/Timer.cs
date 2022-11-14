#nullable enable
using System;
using UniRx;
using UnityEngine.Assertions;

namespace NovemberProject.Time
{
    public sealed class Timer : IReadOnlyTimer
    {
        private readonly Subject<Timer> _onTimerFinished = new();
        private readonly Subject<Timer> _onTimerCanceled = new();
        private readonly Action<Timer>? _callback;

        public float Duration { get; }
        public float Progress { get; private set; }
        public bool IsActive { get; private set; }

        public IObservable<IReadOnlyTimer> OnTimerFinished => _onTimerFinished;
        public IObservable<IReadOnlyTimer> OnTimerCanceled => _onTimerCanceled;

        public float ProgressRate => Progress / Duration;

        public Timer(float duration, Action<Timer>? callback = null)
        {
            Assert.IsTrue(duration > 0);
            Duration = duration;
            _callback = callback;
        }

        public void Start()
        {
            IsActive = true;
        }

        public void Pause()
        {
            IsActive = false;
        }

        public void Cancel()
        {
            IsActive = false;
            _onTimerCanceled.OnNext(this);
        }

        public void AddProgress(float progress)
        {
            Assert.IsTrue(progress >= 0);
            Progress += progress;
            if (Progress >= Duration)
            {
                OnFinished();
            }
        }

        private void OnFinished()
        {
            _callback?.Invoke(this);
            _onTimerFinished.OnNext(this);
        }
    }
}