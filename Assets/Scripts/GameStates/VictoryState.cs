#nullable enable
using System;
using NovemberProject.CoreGameplay;
using NovemberProject.System;
using NovemberProject.Time;
using UniRx;
using Object = UnityEngine.Object;

namespace NovemberProject.GameStates
{
    public sealed class VictoryState : State
    {
        private IDisposable? _sub;
        private TimeSystem _timeSystem;

        public VictoryState(TimeSystem timeSystem)
        {
            _timeSystem = timeSystem;
        }

        protected override void OnEnter()
        {
            Game.Instance.UIManager.HideRoundTimer();
            Game.Instance.UIManager.HideTimeControls();
            _timeSystem.PauseTime();
            Game.Instance.BuildingSelector.Unselect();
            SpaceShipFlightDirector spaceShipFlightDirector = Object.FindObjectOfType<SpaceShipFlightDirector>();
            _sub = spaceShipFlightDirector.OnFinishedPlaying.Subscribe(OnFinishedPlaying);
            spaceShipFlightDirector.StartSequence();
        }

        private void OnFinishedPlaying(Unit _)
        {
            Game.Instance.UIManager.ShowVictoryScreen();
        }

        protected override void OnExit()
        {
            _sub?.Dispose();
            Game.Instance.UIManager.HideVictoryScreen();
        }
    }
}