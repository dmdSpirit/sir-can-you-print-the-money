#nullable enable

using NovemberProject.System;

namespace NovemberProject.GameStates
{
    public sealed class RoundEndState : State
    {
        protected override void OnEnter()
        {
            Game.Instance.RoundSystem.EndRound();
            Game.Instance.UIManager.ShowRoundEndPanel();
            Game.Instance.TimeSystem.PauseTime();
            Game.Instance.UIManager.LockTimeControls();
        }

        protected override void OnExit()
        {
            Game.Instance.UIManager.HideEndOfRoundPanel();
        }
    }
}