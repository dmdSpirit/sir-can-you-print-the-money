#nullable enable
using System;
using NovemberProject.System;
using UniRx;

namespace NovemberProject.GameStates
{
    public sealed class RoundEndState : State
    {
        private IDisposable? _sub;

        protected override void OnEnter()
        {
            Game.Instance.RoundSystem.EndRound();
            Game.Instance.TimeSystem.PauseTime();
            Game.Instance.UIManager.LockTimeControls();
            if (Game.Instance.ResourceMoveEffectSpawner.MoveEffects.Count == 0)
            {
                FinishRound();
                return;
            }

            _sub = Game.Instance.ResourceMoveEffectSpawner.OnEffectsFinished
                .Subscribe(_ => FinishRound());
        }

        protected override void OnExit()
        {
            Game.Instance.UIManager.HideEndOfRoundPanel();
        }

        private void FinishRound()
        {
            _sub?.Dispose();
            Game.Instance.CoreGameplay.EndRound();
            if (Game.Instance.CoreGameplay.IsGameOver())
            {
                Game.Instance.GameStateMachine.GameOver();
                return;
            }

            Game.Instance.UIManager.ShowRoundEndPanel();
        }
    }
}