#nullable enable
using System;
using NovemberProject.GameStates;
using NovemberProject.System.Messages;
using NovemberProject.Time;
using UniRx;
using UnityEngine;
using UnityEngine.Assertions;
using Zenject;

namespace NovemberProject.Rounds
{
    public sealed class RoundSystem
    {
        private const int ROUND_TO_START_FROM = 0;

        private readonly ReactiveProperty<int> _round = new();
        private readonly Subject<Unit> _onRoundEnded = new();
        private readonly Subject<Unit> _onRoundTimerStarted = new();

        private readonly MessageBroker _messageBroker;
        private readonly RoundSystemSettings _settings;
        private readonly TimeSystem _timeSystem;

        private Timer? _roundTimer;

        public IReadOnlyReactiveProperty<int> Round => _round;
        public IObservable<Unit> OnRoundEnded => _onRoundEnded;
        public IObservable<Unit> OnRoundTimerStarted => _onRoundTimerStarted;
        public IReadOnlyTimer? RoundTimer => _roundTimer;

        [Inject]
        public RoundSystem(RoundSystemSettings roundSystemSettings,
            TimeSystem timeSystem, MessageBroker messageBroker)
        {
            _settings = roundSystemSettings;
            _timeSystem = timeSystem;
            _messageBroker = messageBroker;
            _messageBroker.Receive<NewGameMessage>().Subscribe(OnNewGame);
        }

        private void OnNewGame(NewGameMessage message)
        {
            _round.Value = ROUND_TO_START_FROM;
            if (_roundTimer == null)
            {
                return;
            }

            _roundTimer.Cancel();
            _roundTimer = null;
        }

        public void IncrementRound()
        {
            _round.Value++;
        }

        public void StartRound()
        {
            _roundTimer = _timeSystem.CreateTimer(_settings.RoundDuration, OnRoundTimerFinished);
            _roundTimer.Start();

            _onRoundTimerStarted.OnNext(Unit.Default);
        }

        private void OnRoundTimerFinished(IReadOnlyTimer timer)
        {
            Assert.IsTrue(timer == _roundTimer);
            _onRoundEnded.OnNext(Unit.Default);
        }
    }
}