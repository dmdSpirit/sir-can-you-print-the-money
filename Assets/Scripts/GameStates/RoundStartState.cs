#nullable enable
using NovemberProject.System;

namespace NovemberProject.GameStates
{
    public sealed class RoundStartState : State
    {
        protected override void OnEnter()
        {
            Game.Instance.RoundSystem.IncrementRound();
            Game.Instance.UIManager.ShowRoundStartPanel();
        }

        protected override void OnExit()
        {
            Game.Instance.UIManager.HideRoundStartPanel();
        }
    }
}