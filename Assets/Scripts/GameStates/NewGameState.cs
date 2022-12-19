#nullable enable
using NovemberProject.CameraSystem;
using NovemberProject.Rounds;
using NovemberProject.System;
using NovemberProject.System.Messages;
using NovemberProject.Time;
using NovemberProject.Treasures;
using UniRx;

namespace NovemberProject.GameStates
{
    public sealed class NewGameState : State
    {
        private readonly GameStateMachine _gameStateMachine;
        private readonly MessageBroker _messageBroker;
        private readonly TimeSystem _timeSystem;
        private readonly CameraController _cameraController;

        public NewGameState(GameStateMachine gameStateMachine, TimeSystem timeSystem, CameraController cameraController,
            MessageBroker messageBroker)
        {
            _timeSystem = timeSystem;
            _gameStateMachine = gameStateMachine;
            _cameraController = cameraController;
            _messageBroker = messageBroker;
        }

        protected override void OnEnter()
        {
            _timeSystem.ResetTimers();
            _timeSystem.ResetTimeScale();
            _timeSystem.PauseTime();
            _cameraController.InitializeGameData();
            _messageBroker.Publish(new NewGameMessage());
            _gameStateMachine.Tutorial();
        }

        protected override void OnExit()
        {
        }
    }
}