#nullable enable
using NovemberProject.Rounds;
using NovemberProject.System;

namespace NovemberProject.GameStates
{
    public sealed class RoundStartState : State
    {
        private readonly RoundSystem _roundSystem;

        public RoundStartState(RoundSystem roundSystem)
        {
            _roundSystem = roundSystem;
        }

        protected override void OnEnter()
        {
            _roundSystem.IncrementRound();
            Game.Instance.UIManager.ShowTimeControls();
            Game.Instance.UIManager.ShowRoundStartPanel();
        }

        protected override void OnExit()
        {
            Game.Instance.UIManager.HideRoundStartPanel();
        }
    }
}