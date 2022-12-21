#nullable enable
using NovemberProject.Rounds.UI;
using NovemberProject.System.UI;

namespace NovemberProject.GameStates
{
    public sealed class CreditsScreenState : State
    {
        private readonly UIManager _uiManager;

        public CreditsScreenState(UIManager uiManager)
        {
            _uiManager = uiManager;
        }

        protected override void OnEnter()
        {
            var creditsScreen = _uiManager.GetScreen<ICreditsScreen>();
            creditsScreen.Show();
        }

        protected override void OnExit()
        {
            var creditsScreen = _uiManager.GetScreen<ICreditsScreen>();
            creditsScreen.Hide();
        }
    }
}