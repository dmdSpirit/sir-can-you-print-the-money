#nullable enable
using NovemberProject.System;

namespace NovemberProject.GameStates
{
    public sealed class VictoryState : State
    {
        protected override void OnEnter()
        {
            Game.Instance.UIManager.HideRoundTimer();
            Game.Instance.UIManager.HideTimeControls();
            Game.Instance.TimeSystem.PauseTime();
            Game.Instance.UIManager.ShowVictoryScreen();
        }

        protected override void OnExit()
        {
            Game.Instance.UIManager.HideVictoryScreen();
        }
    }
}