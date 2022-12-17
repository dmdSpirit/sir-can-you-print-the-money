#nullable enable
using NovemberProject.System;

namespace NovemberProject.GameStates
{
    public sealed class TechTreeState : State
    {
        protected override void OnEnter()
        {
            Game.Instance.UIManager.ShowTechTreeScreen();
            Game.Instance.TimeSystem.PauseTime();
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