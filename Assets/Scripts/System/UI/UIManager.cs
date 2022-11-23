#nullable enable
using NovemberProject.Buildings;
using NovemberProject.Buildings.UI;
using NovemberProject.Cheats;
using NovemberProject.CommonUIStuff;
using NovemberProject.GameStates;
using NovemberProject.GameStates.UI;
using NovemberProject.Rounds.UI;
using NovemberProject.Time.UI;
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
        private RoundEndPanel _roundEndPanel = null!;

        [SerializeField]
        private BuildingInfoPanel _buildingInfoPanel = null!;

        [SerializeField]
        private RoundStartPanel _roundStartPanel = null!;

        [SerializeField]
        private CheatMenu _cheatMenu = null!;

        [SerializeField]
        private SystemInfoPanel _systemInfoPanel = null!;

        [SerializeField]
        private TimeControlsPanel _timeControlsPanel = null!;

        [SerializeField]
        private GameOverPanel _gameOverPanel = null!;

        [SerializeField]
        private NewGamePanel _newGamePanel = null!;

        [SerializeField]
        private LayerMask _layerMask;

        public LayerMask LayerMask => _layerMask;
        public IReadOnlyReactiveProperty<bool> IsMouseOver => _mouseOverObserver.IsMouseOver;

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
        public void LockTimeControls() => _timeControlsPanel.Lock();
        public void UnlockTimeControls() => _timeControlsPanel.Unlock();
        public void ShowTimeControls() => _timeControlsPanel.Show(null);
        public void HideTimeControls() => _timeControlsPanel.Hide();
        public void ShowRoundEndPanel() => _roundEndPanel.Show(null);
        public void HideEndOfRoundPanel() => _roundEndPanel.Hide();

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
        public void HideCheatPanel() => _cheatMenu.Hide();
        public void HideSystemInfoPanel() => _systemInfoPanel.Hide();
        public void ShowRoundStartPanel() => _roundStartPanel.Show(null);
        public void HideRoundStartPanel() => _roundStartPanel.Hide();
        public void ShowGameOverPanel(GameOverType gameOverType) => _gameOverPanel.Show(gameOverType);
        public void HideGameOverPanel() => _gameOverPanel.Hide();

        public void ShowNewGamePanel() => _newGamePanel.Show(null);
        public void HideNewGamePanel() => _newGamePanel.Hide();
    }
}

// TODO (Stas): #idea Would be great to have Show<UIElement> Hide<UIElement> methods.