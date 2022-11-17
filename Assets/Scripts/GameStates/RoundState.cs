#nullable enable
using NovemberProject.System;

namespace NovemberProject.GameStates
{
    public sealed class RoundState : State
    {
        protected override void OnEnter()
        {
            Game.Instance.RoundSystem.StartRound();
            Game.Instance.UIManager.ShowRoundTimer();
            Game.Instance.UIManager.UnlockTimeControls();
            Game.Instance.TimeSystem.RestoreAfterPause();
            Game.Instance.UIManager.UnlockTimeControls();
        }

        protected override void OnExit()
        {
            Game.Instance.UIManager.LockTimeControls();
            Game.Instance.UIManager.HideRoundTimer();
        }
    }
}