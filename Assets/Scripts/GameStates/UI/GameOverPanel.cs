#nullable enable
using NovemberProject.CommonUIStuff;
using NovemberProject.System;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;
using Zenject;

namespace NovemberProject.GameStates.UI
{
    public interface IGameOverPanel : IUIScreen
    {
        public void SetGameOverType(GameOverType gameOverType);
    }

    public sealed class GameOverPanel : UIScreen, IGameOverPanel
    {
        private GameStateMachine _gameStateMachine = null!;
        private GameOverType? _gameOverType;

        [SerializeField]
        private GameObject _noArmyPanel = null!;

        [SerializeField]
        private GameObject _noFolkPanel = null!;

        [SerializeField]
        private Button _toMainMenuButton = null!;

        [Inject]
        private void Construct(GameStateMachine gameStateMachine)
        {
            _gameStateMachine = gameStateMachine;
        }

        private void Start()
        {
            _toMainMenuButton.onClick.AddListener(ToMainMenuClicked);
        }

        protected override void OnShow()
        {
            if (_gameOverType == null)
            {
                Debug.LogError($"{nameof(_gameOverType)} should be set before showing {nameof(GameOverPanel)}.");
                return;
            }

            _noArmyPanel.SetActive(_gameOverType == GameOverType.NoArmy);
            _noFolkPanel.SetActive(_gameOverType == GameOverType.NoFolk);
        }

        protected override void OnHide()
        {
            _noArmyPanel.SetActive(false);
            _noFolkPanel.SetActive(false);
            _gameOverType = null!;
        }

        public void SetGameOverType(GameOverType gameOverType)
        {
            _gameOverType = gameOverType;
        }

        private void ToMainMenuClicked()
        {
            _gameStateMachine.MainMenu();
        }
    }
}