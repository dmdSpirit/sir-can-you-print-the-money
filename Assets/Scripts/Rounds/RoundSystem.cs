﻿#nullable enable
using System;
using NovemberProject.System;
using NovemberProject.Time;
using UniRx;
using UnityEngine;
using UnityEngine.Assertions;

namespace NovemberProject.Rounds
{
    public sealed class RoundSystem : MonoBehaviour
    {
        private const int ROUND_TO_START_FROM = 0;

        private readonly ReactiveProperty<int> _round = new();
        private readonly Subject<Unit> _onRoundEnded = new();
        private readonly Subject<Unit> _onRoundTimerStarted = new();

        private Timer? _roundTimer;

        [SerializeField]
        private float _roundDuration;

        public IReadOnlyReactiveProperty<int> Round => _round;
        public IObservable<Unit> OnRoundEnded => _onRoundEnded;
        public IObservable<Unit> OnRoundTimerStarted => _onRoundTimerStarted;
        public IReadOnlyTimer? RoundTimer => _roundTimer;

        public void ResetRounds()
        {
            _round.Value = ROUND_TO_START_FROM;
            if (_roundTimer != null)
            {
                _roundTimer.Cancel();
                _roundTimer = null;
            }
        }

        public void IncrementRound()
        {
            _round.Value++;
        }

        public void StartRound()
        {
            _roundTimer = Game.Instance.TimeSystem.CreateTimer(_roundDuration, OnRoundTimerFinished);
            _roundTimer.Start();

            _onRoundTimerStarted.OnNext(Unit.Default);
        }

        public void EndRound()
        {
            _onRoundEnded.OnNext(Unit.Default);
        }

        private void OnRoundTimerFinished(IReadOnlyTimer timer)
        {
            Assert.IsTrue(timer == _roundTimer);
            Game.Instance.GameStateMachine.FinishRound();
        }
    }
}