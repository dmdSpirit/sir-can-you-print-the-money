#nullable enable
using NovemberProject.System;
using NovemberProject.Time;

namespace NovemberProject.GameStates
{
    public sealed class RoundState : State
    {
        private readonly TimeSystem _timeSystem;

        public RoundState(TimeSystem timeSystem)
        {
            _timeSystem = timeSystem;
        }

        protected override void OnEnter()
        {
            Game.Instance.RoundSystem.StartRound();
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