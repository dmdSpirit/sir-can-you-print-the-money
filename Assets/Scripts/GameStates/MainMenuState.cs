#nullable enable
using NovemberProject.CameraSystem;
using NovemberProject.System;
using NovemberProject.Time;

namespace NovemberProject.GameStates
{
    public sealed class MainMenuState : State
    {
        private readonly TimeSystem _timeSystem;
        private readonly CameraController _cameraController;

        public MainMenuState(TimeSystem timeSystem,CameraController cameraController)
        {
            _timeSystem = timeSystem;
            _cameraController = cameraController;
        }

        protected override void OnEnter()
        {
            _cameraController.TurnCameraOff();
            Game.Instance.UIManager.HideRoundTimer();
            Game.Instance.UIManager.HideTimeControls();
            Game.Instance.UIManager.ShowMainMenu();
            _timeSystem.PauseTime();
        }

        protected override void OnExit()
        {
            _cameraController.TurnCameraOn();
            Game.Instance.UIManager.HideMainMenu();
        }
    }
}