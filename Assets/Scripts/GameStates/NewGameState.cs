#nullable enable
using NovemberProject.System;
using NovemberProject.System.Messages;
using UniRx;

namespace NovemberProject.GameStates
{
    public sealed class NewGameState : State
    {
        private readonly GameStateMachine _gameStateMachine;
        private readonly MessageBroker _messageBroker;

        public NewGameState(GameStateMachine gameStateMachine, MessageBroker messageBroker)
        {
            _messageBroker = messageBroker;
            _gameStateMachine = gameStateMachine;
        }

        protected override void OnEnter()
        {
            Game.Instance.TimeSystem.ResetTimers();
            Game.Instance.TimeSystem.ResetTimeScale();
            Game.Instance.TimeSystem.PauseTime();
            Game.Instance.CameraController.InitializeGameData();
            Game.Instance.TreasureController.InitializeGameData();
            Game.Instance.TechController.InitializeGameData();
            Game.Instance.CombatController.InitializeGameData();
            _messageBroker.Publish(new NewGameMessage());
            _gameStateMachine.Tutorial();
        }

        protected override void OnExit()
        {
        }
    }
}