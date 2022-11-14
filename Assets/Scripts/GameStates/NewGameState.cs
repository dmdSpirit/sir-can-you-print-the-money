#nullable enable
using NovemberProject.System;

namespace NovemberProject.GameStates
{
    public sealed class NewGameState : State
    {
        protected override void OnEnter()
        {
            Game.Instance.TimeSystem.ResetTimeScale();
            Game.Instance.RoundSystem.ResetRounds();
            Game.Instance.GameStateMachine.Turn();
        }

        protected override void OnExit()
        {
        }
    }
}