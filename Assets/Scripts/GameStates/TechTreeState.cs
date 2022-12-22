#nullable enable
using NovemberProject.Buildings;
using NovemberProject.Rounds.UI;
using NovemberProject.System;
using NovemberProject.System.UI;
using NovemberProject.Time;
using NovemberProject.Time.UI;

namespace NovemberProject.GameStates
{
    public sealed class TechTreeState : State
    {
        private readonly TimeSystem _timeSystem;
        private readonly UIManager _uiManager;
        private readonly BuildingSelector _buildingSelector;

        private ITechTreeScreen _techTreeScreen = null!;
        private ITimeControlsPanel _timeControlsPanel = null!;

        public TechTreeState(TimeSystem timeSystem, UIManager uiManager, BuildingSelector buildingSelector)
        {
            _timeSystem = timeSystem;
            _uiManager = uiManager;
            _buildingSelector = buildingSelector;
        }

        protected override void OnEnter()
        {
            _techTreeScreen = _uiManager.GetScreen<ITechTreeScreen>();
            _techTreeScreen.Show();
            _timeSystem.PauseTime();
            _timeControlsPanel = _uiManager.GetScreen<ITimeControlsPanel>();
            _timeControlsPanel.Lock();
            _buildingSelector.Unselect();
        }

        protected override void OnExit()
        {
            _techTreeScreen.Hide();
            _timeControlsPanel.Unlock();
        }
    }
}