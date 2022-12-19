#nullable enable
using NovemberProject.CoreGameplay;
using NovemberProject.System;
using NovemberProject.Time;

namespace NovemberProject.GameStates
{
    public sealed class ExpeditionFinishedState : State
    {
        private readonly ExpeditionResult _expeditionResult;
        private readonly TimeSystem _timeSystem;

        public ExpeditionFinishedState(ExpeditionResult expeditionResult, TimeSystem timeSystem)
        {
            _expeditionResult = expeditionResult;
            _timeSystem = timeSystem;
        }

        protected override void OnEnter()
        {
            Game.Instance.UIManager.ShowExpeditionResult(_expeditionResult);
            _timeSystem.PauseTime();
            Game.Instance.UIManager.LockTimeControls();
            Game.Instance.BuildingSelector.Unselect();
        }

        protected override void OnExit()
        {
            Game.Instance.UIManager.HideExpeditionResult();
            _timeSystem.RestoreAfterPause();
            Game.Instance.UIManager.UnlockTimeControls();
        }
    }
}