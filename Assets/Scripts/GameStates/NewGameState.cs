#nullable enable
using NovemberProject.Rounds;
using NovemberProject.System;
using NovemberProject.System.Messages;
using NovemberProject.Time;
using UniRx;

namespace NovemberProject.GameStates
{
    public sealed class NewGameState : State
    {
        private readonly GameStateMachine _gameStateMachine;
        private readonly MessageBroker _messageBroker;
        private readonly TimeSystem _timeSystem;

        public NewGameState(GameStateMachine gameStateMachine, TimeSystem timeSystem, MessageBroker messageBroker)
        {
            _messageBroker = messageBroker;
            _timeSystem = timeSystem;
            _gameStateMachine = gameStateMachine;
        }

        protected override void OnEnter()
        {
            _timeSystem.ResetTimers();
            _timeSystem.ResetTimeScale();
            _timeSystem.PauseTime();
            Game.Instance.CameraController.InitializeGameData();
            Game.Instance.TreasureController.InitializeGameData();
            _messageBroker.Publish(new NewGameMessage());
            _gameStateMachine.Tutorial();
        }

        protected override void OnExit()
        {
        }
    }
}