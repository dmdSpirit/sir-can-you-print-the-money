#nullable enable
using System;
using NovemberProject.MovingResources;
using NovemberProject.Rounds;
using NovemberProject.Rounds.UI;
using NovemberProject.System;
using NovemberProject.System.UI;
using NovemberProject.Time;
using NovemberProject.Time.UI;
using UniRx;

namespace NovemberProject.GameStates
{
    public sealed class RoundEndState : State
    {
        private readonly GameStateMachine _gameStateMachine;
        private readonly TimeSystem _timeSystem;
        private readonly RoundSystem _roundSystem;
        private readonly UIManager _uiManager;
        private readonly ResourceMoveEffectSpawner _resourceMoveEffectSpawner;
        private readonly CoreGameplay.CoreGameplay _coreGameplay;

        private IDisposable? _sub;
        private IRoundEndPanel _roundEndPanel = null!;

        public RoundEndState(GameStateMachine gameStateMachine, TimeSystem timeSystem, RoundSystem roundSystem,
            UIManager uiManager, ResourceMoveEffectSpawner resourceMoveEffectSpawner, CoreGameplay.CoreGameplay coreGameplay)
        {
            _gameStateMachine = gameStateMachine;
            _timeSystem = timeSystem;
            _roundSystem = roundSystem;
            _uiManager = uiManager;
            _resourceMoveEffectSpawner = resourceMoveEffectSpawner;
            _coreGameplay = coreGameplay;
        }

        protected override void OnEnter()
        {
            _timeSystem.PauseTime();
            var timeControlsPanel = _uiManager.GetScreen<ITimeControlsPanel>();
            timeControlsPanel.Lock();
            if (_resourceMoveEffectSpawner.MoveEffects.Count == 0)
            {
                FinishRound();
                return;
            }

            _sub = _resourceMoveEffectSpawner.OnEffectsFinished
                .Subscribe(_ => FinishRound());
        }

        protected override void OnExit()
        {
            _roundEndPanel.Hide();
        }

        private void FinishRound()
        {
            _sub?.Dispose();
            _coreGameplay.EndRound();
            if (_coreGameplay.IsGameOver())
            {
                _gameStateMachine.GameOver();
                return;
            }

            _roundEndPanel = _uiManager.GetScreen<IRoundEndPanel>();
            if (_coreGameplay.RoundResult.NothingHappened())
            {
                _gameStateMachine.StartRound();
                return;
            }
            _roundEndPanel.SetRoundResult(_coreGameplay.RoundResult);
            _roundEndPanel.Show();
        }
    }
}