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
            Game.Instance.TimeSystem.PauseTime();
            Game.Instance.RoundSystem.ResetRounds();
            Game.Instance.CoreGameplay.InitializeGameData();
            Game.Instance.MoneyController.InitializeGameData();
            Game.Instance.FoodController.InitializeGameData();
            Game.Instance.CameraController.InitializeGameData();
            Game.Instance.TreasureController.InitializeGameData();
            Game.Instance.UIManager.ShowNewGamePanel();
        }

        protected override void OnExit()
        {
            Game.Instance.UIManager.HideNewGamePanel();
        }
    }
}