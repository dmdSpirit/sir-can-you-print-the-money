#nullable enable
using NovemberProject.CameraSystem;
using NovemberProject.CommonUIStuff;
using NovemberProject.Rounds.UI;
using UnityEngine;
using UnityEngine.Assertions;

namespace NovemberProject.System.UI
{
    public class UIManager : InitializableBehaviour
    {
        [SerializeField]
        private MainMenu _mainMenu = null!;

        [SerializeField]
        private RoundTimer _roundTimer = null!;

        [SerializeField]
        private EndOfRoundPanel _endOfRoundPanel = null!;

        [SerializeField]
        private BuildingInfoPanel _buildingInfoPanel = null!;

        [SerializeField]
        private LayerMask _layerMask;

        public LayerMask LayerMask => _layerMask;

        protected override void Initialize()
        {
            _mainMenu.Hide();
        }

        public void ShowMainMenu()
        {
            Assert.IsTrue(!_mainMenu.IsShown);
            _mainMenu.Show(null);
        }

        public void HideMainMenu()
        {
            Assert.IsTrue(_mainMenu.IsShown);
            _mainMenu.Hide();
        }

        public void ShowRoundTimer() => _roundTimer.Show(null);
        public void HideRoundTimer() => _roundTimer.Hide();
        public void ShowEndOfRoundPanel() => _endOfRoundPanel.Show(null);
        public void HideEndOfRoundPanel() => _endOfRoundPanel.Hide();

        public void ShowBuildingInfo(Building building)
        {
            if (_buildingInfoPanel.IsShown && _buildingInfoPanel.Building == building)
            {
                return;
            }

            _buildingInfoPanel.Show(building);
        }

        public void HideBuildingInfo()
        {
            _buildingInfoPanel.Hide();
        }
    }
}