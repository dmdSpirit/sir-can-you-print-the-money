#nullable enable
using NovemberProject.System;

namespace NovemberProject.GameStates
{
    public sealed class CreditsScreenState : State
    {
        protected override void OnEnter()
        {
            Game.Instance.UIManager.ShowCreditsScreen();
        }

        protected override void OnExit()
        {
            Game.Instance.UIManager.HideCreditsScreen();
        }
    }
}