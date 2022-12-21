#nullable enable
using NovemberProject.Buildings;
using NovemberProject.Rounds;
using NovemberProject.Rounds.UI;
using NovemberProject.System;
using NovemberProject.System.UI;
using NovemberProject.Time;
using NovemberProject.Time.UI;

namespace NovemberProject.GameStates
{
    public sealed class RoundState : State
    {
        private readonly TimeSystem _timeSystem;
        private readonly RoundSystem _roundSystem;
        private readonly UIManager _uiManager;
        private readonly CoreGameplay.CoreGameplay _coreGameplay;
        private readonly BuildingSelector _buildingSelector;
        private readonly BuildingNameHover _buildingNameHover;

        private ITimeControlsPanel _timeControlsPanel = null!;
        private IRoundTimer _roundTimer = null!;

        public RoundState(TimeSystem timeSystem, RoundSystem roundSystem, UIManager uiManager,
            CoreGameplay.CoreGameplay coreGameplay, BuildingSelector buildingSelector,
            BuildingNameHover buildingNameHover)
        {
            _timeSystem = timeSystem;
            _roundSystem = roundSystem;
            _uiManager = uiManager;
            _coreGameplay = coreGameplay;
            _buildingSelector = buildingSelector;
            _buildingNameHover = buildingNameHover;
        }

        protected override void OnEnter()
        {
            _roundSystem.StartRound();
            _roundTimer = _uiManager.GetScreen<IRoundTimer>();
            _roundTimer.Show();
            _timeControlsPanel = _uiManager.GetScreen<ITimeControlsPanel>();
            _timeControlsPanel.Unlock();
            _timeSystem.RestoreAfterPause();
            _coreGameplay.StartRound();
        }

        protected override void OnExit()
        {
            _buildingSelector.Unselect();
            _timeControlsPanel.Lock();
            _roundTimer.Hide();
            _buildingNameHover.HidePanel();
        }
    }
}