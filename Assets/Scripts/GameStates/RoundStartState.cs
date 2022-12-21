#nullable enable
using NovemberProject.Rounds;
using NovemberProject.Rounds.UI;
using NovemberProject.System.UI;
using NovemberProject.Time.UI;

namespace NovemberProject.GameStates
{
    public sealed class RoundStartState : State
    {
        private readonly RoundSystem _roundSystem;
        private readonly UIManager _uiManager;

        private IRoundStartPanel _roundStartPanel = null!;

        public RoundStartState(RoundSystem roundSystem, UIManager uiManager)
        {
            _roundSystem = roundSystem;
            _uiManager = uiManager;
        }

        protected override void OnEnter()
        {
            _roundSystem.IncrementRound();
            _roundStartPanel = _uiManager.GetScreen<IRoundStartPanel>();
            _roundStartPanel.Show();
            var timeControlsPanel = _uiManager.GetScreen<ITimeControlsPanel>();
            timeControlsPanel.Show();
        }

        protected override void OnExit()
        {
            _roundStartPanel.Hide();
        }
    }
}