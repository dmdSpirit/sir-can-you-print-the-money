#nullable enable
using NovemberProject.Core;
using NovemberProject.GameStates.UI;
using NovemberProject.System;
using NovemberProject.System.UI;

namespace NovemberProject.GameStates
{
    public sealed class GameOverState : State
    {
        private readonly UIManager _uiManager;
        private readonly CoreGameplay _coreGameplay;

        private IGameOverPanel _gameOverPanel = null!;

        public GameOverState(UIManager uiManager, CoreGameplay coreGameplay)
        {
            _uiManager = uiManager;
            _coreGameplay = coreGameplay;
        }

        protected override void OnEnter()
        {
            _gameOverPanel = _uiManager.GetScreen<IGameOverPanel>();
            _gameOverPanel.SetGameOverType(_coreGameplay.GameOverType);
            _gameOverPanel.Show();
        }

        protected override void OnExit()
        {
            _gameOverPanel.Hide();
        }
    }
}