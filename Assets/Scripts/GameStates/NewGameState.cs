#nullable enable
using NovemberProject.System;

namespace NovemberProject.GameStates
{
    public sealed class NewGameState : State
    {
        protected override void OnEnter()
        {
            Game.Instance.TimeSystem.ResetTimers();
            Game.Instance.TimeSystem.ResetTimeScale();
            Game.Instance.RoundSystem.ResetRounds();
            Game.Instance.CoreGameplay.InitializeGameData();
            Game.Instance.MoneyController.InitializeGameData();
            Game.Instance.FoodController.InitializeGameData();
            Game.Instance.GameStateMachine.StartRound();
        }

        protected override void OnExit()
        {
        }
    }
}