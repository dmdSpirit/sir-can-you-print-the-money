#nullable enable
using NovemberProject.Rounds.UI;
using NovemberProject.System;
using NovemberProject.System.UI;

namespace NovemberProject.GameStates
{
    public sealed class TutorialState : State
    {
        private readonly UIManager _uiManager;

        private ITutorialScreen _tutorialScreen = null!;

        public TutorialState(UIManager uiManager)
        {
            _uiManager = uiManager;
        }

        protected override void OnEnter()
        {
            _tutorialScreen = _uiManager.GetScreen<ITutorialScreen>();
            _tutorialScreen.Show();
        }

        protected override void OnExit()
        {
            _tutorialScreen.Hide();
        }
    }
}