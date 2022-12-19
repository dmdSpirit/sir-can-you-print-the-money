#nullable enable
using NovemberProject.System;
using NovemberProject.Time;

namespace NovemberProject.GameStates
{
    public sealed class MainMenuState : State
    {
        private TimeSystem _timeSystem;

        public MainMenuState(TimeSystem timeSystem)
        {
            _timeSystem = timeSystem;
        }

        protected override void OnEnter()
        {
            Game.Instance.CameraController.TurnCameraOff();
            Game.Instance.UIManager.HideRoundTimer();
            Game.Instance.UIManager.HideTimeControls();
            Game.Instance.UIManager.ShowMainMenu();
            _timeSystem.PauseTime();
        }

        protected override void OnExit()
        {
            Game.Instance.CameraController.TurnCameraOn();
            Game.Instance.UIManager.HideMainMenu();
        }
    }
}