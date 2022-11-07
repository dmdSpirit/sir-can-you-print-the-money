#nullable enable
using System;

namespace NovemberProject.Time
{
    public interface IReadOnlyTimer
    {
        public IObservable<IReadOnlyTimer> OnTimerFinished { get; }
        public IObservable<IReadOnlyTimer> OnTimerCanceled { get; }
        public float Duration { get; }
        public float Progress { get; }
        public float ProgressRate { get; }
        public bool IsActive { get; }
    }
}