#nullable enable
using NovemberProject.System;

namespace NovemberProject.GameStates
{
    public sealed class TutorialState : State
    {
        protected override void OnEnter()
        {
            Game.Instance.UIManager.ShowTutorialScreen();
        }

        protected override void OnExit()
        {
            Game.Instance.UIManager.HideTutorialScreen();
        }
    }
}