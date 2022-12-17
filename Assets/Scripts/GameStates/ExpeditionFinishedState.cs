#nullable enable
using NovemberProject.CoreGameplay;
using NovemberProject.System;

namespace NovemberProject.GameStates
{
    public sealed class ExpeditionFinishedState : State
    {
        private readonly ExpeditionResult _expeditionResult;

        public ExpeditionFinishedState(ExpeditionResult expeditionResult)
        {
            _expeditionResult = expeditionResult;
        }

        protected override void OnEnter()
        {
            Game.Instance.UIManager.ShowExpeditionResult(_expeditionResult);
            Game.Instance.TimeSystem.PauseTime();
            Game.Instance.UIManager.LockTimeControls();
            Game.Instance.BuildingSelector.Unselect();
        }

        protected override void OnExit()
        {
            Game.Instance.UIManager.HideExpeditionResult();
            Game.Instance.TimeSystem.RestoreAfterPause();
            Game.Instance.UIManager.UnlockTimeControls();
        }
    }
}