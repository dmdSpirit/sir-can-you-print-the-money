#nullable enable
using NovemberProject.System;

namespace NovemberProject.GameStates
{
    public class RoundState : State
    {
        protected override void OnEnter()
        {
            Game.Instance.RoundSystem.StartRound();
            Game.Instance.UIManager.ShowRoundTimer();
            Game.Instance.UIManager.UnlockBuildingInfoShowing();
            Game.Instance.TimeSystem.RestoreAfterPause();
        }

        protected override void OnExit()
        {
            Game.Instance.UIManager.LockBuildingInfoShowing();
        }
    }
}