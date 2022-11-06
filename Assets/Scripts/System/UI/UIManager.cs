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

        protected override void Initialize()
        {
            _mainMenu.Hide();
        }

        public void ShowMainMenu()
        {
            Assert.IsTrue(!_mainMenu.IsShown);
            _mainMenu.Show();
        }

        public void HideMainMenu()
        {
            Assert.IsTrue(_mainMenu.IsShown);
            _mainMenu.Hide();
        }
    }
}