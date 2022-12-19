#nullable enable
using NovemberProject.CoreGameplay;
using NovemberProject.System;
using NovemberProject.Time;

namespace NovemberProject.GameStates
{
    public sealed class AttackState : State
    {
        private readonly AttackData _attackData;
        private readonly TimeSystem _timeSystem;

        public AttackState(AttackData attackData, TimeSystem timeSystem)
        {
            _attackData = attackData;
            _timeSystem = timeSystem;
        }

        protected override void OnEnter()
        {
            Game.Instance.UIManager.ShowAttackResultsPanel(_attackData);
            _timeSystem.PauseTime();
            Game.Instance.UIManager.LockTimeControls();
            Game.Instance.BuildingSelector.Unselect();
        }

        protected override void OnExit()
        {
            Game.Instance.UIManager.HideAttackResultsPanel();
            _timeSystem.RestoreAfterPause();
            Game.Instance.UIManager.UnlockTimeControls();
        }
    }
}