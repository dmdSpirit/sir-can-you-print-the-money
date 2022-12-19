#nullable enable
using System;
using NovemberProject.System;
using NovemberProject.Time;
using UniRx;

namespace NovemberProject.GameStates
{
    public sealed class RoundEndState : State
    {
        private readonly GameStateMachine _gameStateMachine;
        private readonly TimeSystem _timeSystem;
        
        private IDisposable? _sub;

        public RoundEndState(GameStateMachine gameStateMachine, TimeSystem timeSystem)
        {
            _gameStateMachine = gameStateMachine;
            _timeSystem = timeSystem;
        }

        protected override void OnEnter()
        {
            Game.Instance.RoundSystem.EndRound();
            _timeSystem.PauseTime();
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
                _gameStateMachine.GameOver();
                return;
            }

            Game.Instance.UIManager.ShowRoundEndPanel(Game.Instance.CoreGameplay.RoundResult);
        }
    }
}