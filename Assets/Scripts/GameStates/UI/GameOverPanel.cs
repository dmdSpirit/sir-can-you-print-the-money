#nullable enable
using NovemberProject.CommonUIStuff;
using NovemberProject.System;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace NovemberProject.GameStates.UI
{
    public sealed class GameOverPanel : UIElement<GameOverType>
    {
        [SerializeField]
        private GameObject _noArmyPanel = null!;

        [SerializeField]
        private GameObject _noFolkPanel = null!;

        [SerializeField]
        private Button _toMainMenuButton = null!;

        private void Start()
        {
            _toMainMenuButton.onClick.AddListener(ToMainMenuClicked);
        }

        protected override void OnShow(GameOverType gameOverType)
        {
            Assert.IsTrue(gameOverType is GameOverType.NoArmy or GameOverType.NoFolk);
            _noArmyPanel.SetActive(gameOverType == GameOverType.NoArmy);
            _noFolkPanel.SetActive(gameOverType == GameOverType.NoFolk);
        }

        protected override void OnHide()
        {
            _noArmyPanel.SetActive(false);
            _noFolkPanel.SetActive(false);
        }

        private void ToMainMenuClicked()
        {
            Game.Instance.GameStateMachine.MainMenu();
        }
    }
}