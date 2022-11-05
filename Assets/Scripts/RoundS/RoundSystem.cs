#nullable enable
using System;
using UniRx;
using UnityEngine;

namespace NovemberProject.RoundS
{
    public class RoundSystem : MonoBehaviour
    {
        private readonly ReactiveProperty<int> _round = new();
        private readonly Subject<Unit> _onRoundEnded = new();
        private readonly Subject<Unit> _onRoundStarted = new();

        [SerializeField]
        private int _roundToStartFrom = 1;

        public IReadOnlyReactiveProperty<int> Round => _round;
        public IObservable<Unit> OnRoundEnded => _onRoundEnded;
        public IObservable<Unit> OnRoundStarted => _onRoundStarted;

        public void Initialize()
        {
            _round.Value = _roundToStartFrom;
        }

        public void StartRound()
        {
            _onRoundEnded.OnNext(Unit.Default);
            _round.Value++;
            _onRoundStarted.OnNext(Unit.Default);
        }
    }
}