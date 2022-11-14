#nullable enable
using NovemberProject.Buildings;
using NovemberProject.Buildings.UI;
using NovemberProject.Cheats;
using NovemberProject.CommonUIStuff;
using NovemberProject.Rounds.UI;
using UniRx;
using UnityEngine;
using UnityEngine.Assertions;

namespace NovemberProject.System.UI
{
    [RequireComponent(typeof(MouseOverObserver))]
    public sealed class UIManager : InitializableBehaviour
    {
        private MouseOverObserver _mouseOverObserver = null!;

        [SerializeField]
        private MainMenu _mainMenu = null!;

        [SerializeField]
        private RoundTimer _roundTimer = null!;

        [SerializeField]
        private EndOfRoundPanel _endOfRoundPanel = null!;

        [SerializeField]
        private BuildingInfoPanel _buildingInfoPanel = null!;

        [SerializeField]
        private CheatMenu _cheatMenu = null!;

        [SerializeField]
        private SystemInfoPanel _systemInfoPanel = null!;

        [SerializeField]
        private LayerMask _layerMask;

        public LayerMask LayerMask => _layerMask;
        public IReadOnlyReactiveProperty<bool> IsMouseOver => _mouseOverObserver.IsMouseOver;

        protected override void Initialize()
        {
        }

        private void Awake()
        {
            _mouseOverObserver = GetComponent<MouseOverObserver>();
        }

        public void ShowMainMenu()
        {
            Assert.IsTrue(!_mainMenu.IsShown);
            _mainMenu.Show(null);
        }

        public void HideMainMenu()
        {
            Assert.IsTrue(_mainMenu.IsShown || _mainMenu.gameObject.activeInHierarchy);
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

        public void ToggleSystemInfoPanel()
        {
            if (_systemInfoPanel.IsShown)
            {
                _systemInfoPanel.Hide();
            }
            else
            {
                _systemInfoPanel.Show(null);
            }
        }

        public void ToggleCheatMenu()
        {
            if (_cheatMenu.IsShown)
            {
                _cheatMenu.Hide();
            }
            else
            {
                _cheatMenu.Show(null);
            }
        }

        public void ShowSystemInfoPanel() => _systemInfoPanel.Show(null);
        public void ShowCheatPanel() => _cheatMenu.Show(null);
        public void HideSystemInfoPanel() => _systemInfoPanel.Hide();
        public void HideCheatPanel() => _cheatMenu.Hide();
    }
}

// TODO (Stas): #idea Would be great to have Show<UIElement> Hide<UIElement> methods.