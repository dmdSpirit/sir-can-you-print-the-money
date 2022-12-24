#nullable enable
using System;
using NovemberProject.Buildings;
using NovemberProject.Core;
using NovemberProject.Rounds.UI;
using NovemberProject.System;
using NovemberProject.System.UI;
using NovemberProject.Time;
using NovemberProject.Time.UI;
using UniRx;
using Object = UnityEngine.Object;

namespace NovemberProject.GameStates
{
    public sealed class VictoryState : State
    {
        private readonly TimeSystem _timeSystem;
        private readonly UIManager _uiManager;
        private readonly BuildingSelector _buildingSelector;

        private IDisposable? _sub;
        private IVictoryScreen _victoryScreen = null!;

        public VictoryState(TimeSystem timeSystem, UIManager uiManager, BuildingSelector buildingSelector)
        {
            _timeSystem = timeSystem;
            _uiManager = uiManager;
        }

        protected override void OnEnter()
        {
            var roundTimer = _uiManager.GetScreen<IRoundTimer>();
            roundTimer.Hide();
            var timeControlsPanel = _uiManager.GetScreen<ITimeControlsPanel>();
            timeControlsPanel.Hide();
            _timeSystem.PauseTime();
            _buildingSelector.Unselect();
            SpaceShipFlightDirector spaceShipFlightDirector = Object.FindObjectOfType<SpaceShipFlightDirector>();
            _sub = spaceShipFlightDirector.OnFinishedPlaying.Subscribe(OnFinishedPlaying);
            spaceShipFlightDirector.StartSequence();
        }

        private void OnFinishedPlaying(Unit _)
        {
            _victoryScreen = _uiManager.GetScreen<IVictoryScreen>();
            _victoryScreen.Show();
        }

        protected override void OnExit()
        {
            _sub?.Dispose();
            _victoryScreen.Hide();
        }
    }
}