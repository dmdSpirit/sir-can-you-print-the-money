#nullable enable
using System;
using NovemberProject.Time;
using UniRx;

namespace NovemberProject.Buildings.UI
{
    public interface IIncomingAttack
    {
        public IReadOnlyReactiveProperty<int> Defenders { get; }
        public IReadOnlyTimer? AttackTimer { get; }
        public int Attackers { get; }
        public float WinProbability { get; }
        public IObservable<Unit> OnNewAttack { get; }
    }
}