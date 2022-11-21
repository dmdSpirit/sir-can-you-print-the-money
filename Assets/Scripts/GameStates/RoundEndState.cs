#nullable enable
using NovemberProject.System;

namespace NovemberProject.GameStates
{
    public sealed class RoundEndState : State
    {
        protected override void OnEnter()
        {
            Game.Instance.RoundSystem.EndRound();
            Game.Instance.TimeSystem.PauseTime();
            Game.Instance.UIManager.LockTimeControls();
            Game.Instance.CoreGameplay.EndRound();
            if (Game.Instance.CoreGameplay.IsGameOver())
            {
                Game.Instance.GameStateMachine.GameOver();
                return;
            }
            Game.Instance.UIManager.ShowRoundEndPanel();
        }

        protected override void OnExit()
        {
            Game.Instance.UIManager.HideEndOfRoundPanel();
        }
    }
}