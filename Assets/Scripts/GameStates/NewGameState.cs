#nullable enable
using NovemberProject.System;

namespace NovemberProject.GameStates
{
    public class NewGameState : State
    {
        protected override void OnEnter()
        {
            Game.Instance.TimeSystem.ResetTimeScale();
            Game.Instance.GameStateMachine.Turn();
        }

        protected override void OnExit()
        {
        }
    }
}