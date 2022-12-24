#nullable enable
using NovemberProject.Buildings;
using NovemberProject.Core;
using NovemberProject.GameStates.UI;
using NovemberProject.System.UI;
using NovemberProject.Time;
using NovemberProject.Time.UI;

namespace NovemberProject.GameStates
{
    public sealed class ExpeditionFinishedState : State
    {
        private readonly ExpeditionResult _expeditionResult;
        private readonly TimeSystem _timeSystem;
        private readonly UIManager _uiManager;
        private readonly BuildingSelector _buildingSelector;

        private IExpeditionResultsPanel _expeditionResultsPanel = null!;
        private ITimeControlsPanel _timeControlsPanel = null!;

        public ExpeditionFinishedState(ExpeditionResult expeditionResult, TimeSystem timeSystem, UIManager uiManager,
            BuildingSelector buildingSelector)
        {
            _expeditionResult = expeditionResult;
            _timeSystem = timeSystem;
            _uiManager = uiManager;
            _buildingSelector = buildingSelector;
        }

        protected override void OnEnter()
        {
            _expeditionResultsPanel = _uiManager.GetScreen<IExpeditionResultsPanel>();
            _expeditionResultsPanel.SetExpeditionResult(_expeditionResult);
            _expeditionResultsPanel.Show();
            _timeSystem.PauseTime();
            _timeControlsPanel = _uiManager.GetScreen<ITimeControlsPanel>();
            _timeControlsPanel.Lock();
            _buildingSelector.Unselect();
        }

        protected override void OnExit()
        {
            _expeditionResultsPanel.Hide();
            _timeSystem.RestoreAfterPause();
            _timeControlsPanel.Unlock();
        }
    }
}