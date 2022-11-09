#nullable enable
using NovemberProject.System;

namespace NovemberProject.GameStates
{
    public class InitializeGameState : State
    {
        protected override void OnEnter()
        {
            Game.Instance.UIManager.HideRoundTimer();
            Game.Instance.UIManager.HideEndOfRoundPanel();
            Game.Instance.MainMenu();
            Game.Instance.UIManager.HideBuildingInfo();
        }

        protected override void OnExit()
        {
        }
    }
}