#nullable enable
using NovemberProject.GameStates.UI;
using NovemberProject.System;
using NovemberProject.System.UI;

namespace NovemberProject.GameStates
{
    public sealed class GameOverState : State
    {
        private readonly UIManager _uiManager;

        private IGameOverPanel _gameOverPanel = null!;

        public GameOverState(UIManager uiManager)
        {
            _uiManager = uiManager;
        }

        protected override void OnEnter()
        {
            _gameOverPanel = _uiManager.GetScreen<IGameOverPanel>();
            _gameOverPanel.Show();
        }

        protected override void OnExit()
        {
            _gameOverPanel.Hide();
        }
    }
}