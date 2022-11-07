#nullable enable
using NovemberProject.CommonUIStuff;
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

        public void ShowRoundTimer()
        {
            _roundTimer.Show(null);
        }

        public void HideRoundTimer()
        {
            _roundTimer.Hide();
        }

        public void ShowEndOfRoundPanel()
        {
            _endOfRoundPanel.Show(null);
        }

        public void HideEndOfRoundPanel()
        {
            _endOfRoundPanel.Hide();
        }
    }
}