#nullable enable
using NovemberProject.System;
using NovemberProject.System.Messages;

namespace NovemberProject.GameStates
{
    public sealed class NewGameState : State
    {
        protected override void OnEnter()
        {
            Game.Instance.TimeSystem.ResetTimers();
            Game.Instance.TimeSystem.ResetTimeScale();
            Game.Instance.TimeSystem.PauseTime();
            Game.Instance.RoundSystem.ResetRounds();
            Game.Instance.CoreGameplay.InitializeGameData();
            Game.Instance.StoneController.InitializeGameData();
            Game.Instance.CameraController.InitializeGameData();
            Game.Instance.TreasureController.InitializeGameData();
            Game.Instance.TechController.InitializeGameData();
            Game.Instance.CombatController.InitializeGameData();
            Game.Instance.MessageBroker.Publish(new NewGameMessage());
            Game.Instance.GameStateMachine.Tutorial();
        }

        protected override void OnExit()
        {
        }
    }
}