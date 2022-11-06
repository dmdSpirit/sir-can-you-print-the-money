#nullable enable
using NovemberProject.System;

namespace NovemberProject.GameStates
{
    public class MainMenuState : State
    {
        protected override void OnEnter()
        {
            Game.Instance.UIManager.ShowMainMenu();
        }

        protected override void OnExit()
        {
            Game.Instance.UIManager.HideMainMenu();
        }
    }
}