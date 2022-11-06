#nullable enable
using System;
using UniRx;
using UnityEngine.Assertions;

namespace NovemberProject.TimeSystem
{
    public class Timer
    {
        private readonly Subject<Timer> _onTimerFinished = new();
        private readonly Subject<Timer> _onTimerCanceled = new();
        private readonly Action<Timer>? _callback;

        public readonly float Duration;

        public float Progress { get; private set; }
        public bool IsActive { get; private set; }

        public IObservable<Timer> OnTimerFinished => _onTimerFinished;
        public IObservable<Timer> OnTimerCanceled => _onTimerCanceled;

        public Timer(float duration, Action<Timer>? callback = null)
        {
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