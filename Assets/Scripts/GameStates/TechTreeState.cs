#nullable enable
using NovemberProject.System;
using NovemberProject.Time;

namespace NovemberProject.GameStates
{
    public sealed class TechTreeState : State
    {
        private readonly TimeSystem _timeSystem;

        public TechTreeState(TimeSystem timeSystem)
        {
            _timeSystem = timeSystem;
        }

        protected override void OnEnter()
        {
            Game.Instance.UIManager.ShowTechTreeScreen();
            _timeSystem.PauseTime();
            Game.Instance.UIManager.LockTimeControls();
            Game.Instance.BuildingSelector.Unselect();
        }

        protected override void OnExit()
        {
            Game.Instance.UIManager.HideTechTreeScreen();
            Game.Instance.UIManager.UnlockTimeControls();
        }
    }
}