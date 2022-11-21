#nullable enable
using NovemberProject.System;

namespace NovemberProject.GameStates
{
    public sealed class GameOverState : State
    {
        protected override void OnEnter()
        {
            Game.Instance.UIManager.ShowGameOverPanel(Game.Instance.CoreGameplay.GameOverType);
        }

        protected override void OnExit()
        {
            Game.Instance.UIManager.HideGameOverPanel();
        }
    }
}