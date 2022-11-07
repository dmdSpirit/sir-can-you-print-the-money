#nullable enable
using NovemberProject.System;

namespace NovemberProject.GameStates
{
    public class RoundState : State
    {
        protected override void OnEnter()
        {
            Game.Instance.RoundSystem.StartRound();
            Game.Instance.UIManager.ShowRoundTimer();
        }

        protected override void OnExit()
        {
        }
    }
}