#nullable enable
using System;
using NovemberProject.System;
using NovemberProject.Time;
using UniRx;
using UnityEngine;
using UnityEngine.Assertions;

namespace NovemberProject.RoundS
{
    public class RoundSystem : MonoBehaviour
    {
        private const int ROUND_TO_START_FROM = 0;

        private readonly ReactiveProperty<int> _round = new();
        private readonly Subject<Unit> _onRoundEnded = new();
        private readonly Subject<Unit> _onRoundStarted = new();

        private Timer? _roundTimer;
        private bool _isRoundActive;

        [SerializeField]
        private float _roundDuration;

        public IReadOnlyReactiveProperty<int> Round => _round;
        public IObservable<Unit> OnRoundEnded => _onRoundEnded;
        public IObservable<Unit> OnRoundStarted => _onRoundStarted;
        public IReadOnlyTimer? RoundTimer => _roundTimer;

        private void Awake()
        {
            Debug.Log(_roundDuration);
        }

        public void ResetRounds()
        {
            _round.Value = ROUND_TO_START_FROM;
        }

        public void StartRound()
        {
            _onRoundEnded.OnNext(Unit.Default);
            _round.Value++;
            _roundTimer = Game.Instance.TimeSystem.CreateTimer(_roundDuration, OnRoundTimerFinished);
            _roundTimer.Start();
            _isRoundActive = true;

            _onRoundStarted.OnNext(Unit.Default);
        }

        public void EndRound()
        {
            _isRoundActive = false;
        }

        private void OnRoundTimerFinished(IReadOnlyTimer timer)
        {
            Assert.IsTrue(timer == _roundTimer);
            Game.Instance.FinishRound();
        }
    }
}