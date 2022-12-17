#nullable enable
using System;
using NovemberProject.GameStates;
using NovemberProject.System;
using NovemberProject.System.Messages;
using NovemberProject.Time;
using UniRx;
using UnityEngine;
using UnityEngine.Assertions;
using Zenject;

namespace NovemberProject.Rounds
{
    public sealed class RoundSystem : MonoBehaviour
    {
        private const int ROUND_TO_START_FROM = 0;

        private readonly ReactiveProperty<int> _round = new();
        private readonly Subject<Unit> _onRoundEnded = new();
        private readonly Subject<Unit> _onRoundTimerStarted = new();

        private GameStateMachine _gameStateMachine = null!;
        private MessageBroker _messageBroker = null!;

        private Timer? _roundTimer;

        [SerializeField]
        private float _roundDuration;

        public IReadOnlyReactiveProperty<int> Round => _round;
        public IObservable<Unit> OnRoundEnded => _onRoundEnded;
        public IObservable<Unit> OnRoundTimerStarted => _onRoundTimerStarted;
        public IReadOnlyTimer? RoundTimer => _roundTimer;

        [Inject]
        private void Construct(GameStateMachine gameStateMachine, MessageBroker messageBroker)
        {
            _gameStateMachine = gameStateMachine;
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
            _gameStateMachine.FinishRound();
        }
    }
}