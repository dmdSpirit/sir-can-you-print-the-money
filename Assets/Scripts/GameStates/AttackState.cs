#nullable enable
using NovemberProject.Buildings;
using NovemberProject.CoreGameplay;
using NovemberProject.GameStates.UI;
using NovemberProject.System.UI;
using NovemberProject.Time;
using NovemberProject.Time.UI;

namespace NovemberProject.GameStates
{
    public sealed class AttackState : State
    {
        private readonly AttackData _attackData;
        private readonly TimeSystem _timeSystem;
        private readonly UIManager _uiManager;
        private readonly BuildingSelector _buildingSelector;

        private ITimeControlsPanel _timeControlsPanel = null!;
        private IAttackResultsPanel _attackResultsPanel = null!;

        public AttackState(AttackData attackData, TimeSystem timeSystem, UIManager uiManager,
            BuildingSelector buildingSelector)
        {
            _attackData = attackData;
            _timeSystem = timeSystem;
            _uiManager = uiManager;
            _buildingSelector = buildingSelector;
        }

        protected override void OnEnter()
        {
            _attackResultsPanel = _uiManager.GetScreen<IAttackResultsPanel>();
            _attackResultsPanel.SetAttackData(_attackData);
            _attackResultsPanel.Show();
            _timeSystem.PauseTime();
            _timeControlsPanel = _uiManager.GetScreen<ITimeControlsPanel>();
            _timeControlsPanel.Lock();
            _buildingSelector.Unselect();
        }

        protected override void OnExit()
        {
            _attackResultsPanel.Hide();
            _timeSystem.RestoreAfterPause();
            _timeControlsPanel.Unlock();
        }
    }
}