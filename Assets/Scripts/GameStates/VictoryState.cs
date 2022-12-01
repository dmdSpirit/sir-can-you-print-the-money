#nullable enable
using System;
using NovemberProject.CoreGameplay;
using NovemberProject.System;
using UniRx;
using Object = UnityEngine.Object;

namespace NovemberProject.GameStates
{
    public sealed class VictoryState : State
    {
        private IDisposable? _sub;

        protected override void OnEnter()
        {
            Game.Instance.UIManager.HideRoundTimer();
            Game.Instance.UIManager.HideTimeControls();
            Game.Instance.TimeSystem.PauseTime();
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