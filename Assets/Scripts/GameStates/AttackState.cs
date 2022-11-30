#nullable enable
using NovemberProject.CoreGameplay;
using NovemberProject.System;

namespace NovemberProject.GameStates
{
    public sealed class AttackState : State
    {
        private readonly AttackData _attackData;

        public AttackState(AttackData attackData)
        {
            _attackData = attackData;
        }
        protected override void OnEnter()
        {
            Game.Instance.UIManager.ShowAttackResultsPanel(_attackData);
            Game.Instance.TimeSystem.PauseTime();
            Game.Instance.UIManager.LockTimeControls();
            Game.Instance.BuildingSelector.Unselect();
            Game.Instance.ArmyManager.ReturnExplorersToGuard();
        }

        protected override void OnExit()
        {
            Game.Instance.UIManager.HideAttackResultsPanel();
            Game.Instance.TimeSystem.RestoreAfterPause();
            Game.Instance.UIManager.UnlockTimeControls();
        }
    }
}