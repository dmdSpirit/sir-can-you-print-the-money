#nullable enable
using NovemberProject.Rounds;
using NovemberProject.System;
using NovemberProject.Time;

namespace NovemberProject.GameStates
{
    public sealed class RoundState : State
    {
        private readonly TimeSystem _timeSystem;
        private readonly RoundSystem _roundSystem;

        public RoundState(TimeSystem timeSystem, RoundSystem roundSystem)
        {
            _timeSystem = timeSystem;
            _roundSystem = roundSystem;
        }

        protected override void OnEnter()
        {
            _roundSystem.StartRound();
            Game.Instance.UIManager.ShowRoundTimer();
            Game.Instance.UIManager.UnlockTimeControls();
            _timeSystem.RestoreAfterPause();
            Game.Instance.UIManager.UnlockTimeControls();
            Game.Instance.CoreGameplay.StartRound();
        }

        protected override void OnExit()
        {
            Game.Instance.BuildingSelector.Unselect();
            Game.Instance.UIManager.LockTimeControls();
            Game.Instance.UIManager.HideRoundTimer();
            Game.Instance.BuildingNameHover.HidePanel();
        }
    }
}