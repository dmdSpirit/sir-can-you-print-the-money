#nullable enable

using NovemberProject.System;

namespace NovemberProject.GameStates
{
    public class EndOnRoundState : State
    {
        protected override void OnEnter()
        {
            Game.Instance.RoundSystem.EndRound();
            Game.Instance.UIManager.ShowEndOfRoundPanel();
            Game.Instance.TimeSystem.PauseTime();
        }

        protected override void OnExit()
        {
            Game.Instance.UIManager.HideEndOfRoundPanel();
        }
    }
}