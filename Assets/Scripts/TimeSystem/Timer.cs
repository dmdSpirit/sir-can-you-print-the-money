﻿#nullable enable
using System;
using UniRx;
using UnityEngine.Assertions;

namespace NovemberProject.TimeSystem
{
    public class Timer
    {
        private readonly Subject<Timer> _onTimerFinished = new();
        private readonly Subject<Timer> _onTimerCanceled = new();

        public readonly float Duration;

        public float Progress { get; private set; }
        public bool IsActive { get; private set; }

        public IObservable<Timer> OnTimerFinished => _onTimerFinished;
        public IObservable<Timer> OnTimerCanceled => _onTimerCanceled;

        public Timer(float duration)
        {
            Duration = duration;
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
            Assert.IsTrue(progress > 0);
            Progress += progress;
            if (Progress >= Duration)
            {
                OnFinished();
            }
        }

        private void OnFinished()
        {
            _onTimerFinished.OnNext(this);
        }
    }
}