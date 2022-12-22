#nullable enable
using NovemberProject.CameraSystem;
using NovemberProject.Rounds.UI;
using NovemberProject.System.UI;
using NovemberProject.Time;
using NovemberProject.Time.UI;

namespace NovemberProject.GameStates
{
    public sealed class MainMenuState : State
    {
        private readonly TimeSystem _timeSystem;
        private readonly CameraController _cameraController;
        private readonly UIManager _uiManager;

        private IMainMenu _mainMenu = null!;

        public MainMenuState(TimeSystem timeSystem, CameraController cameraController, UIManager uiManager)
        {
            _timeSystem = timeSystem;
            _cameraController = cameraController;
            _uiManager = uiManager;
        }

        protected override void OnEnter()
        {
            _cameraController.TurnCameraOff();
            var roundTimer = _uiManager.GetScreen<IRoundTimer>();
            roundTimer.Hide();
            var timeControlsPanel = _uiManager.GetScreen<ITimeControlsPanel>();
            timeControlsPanel.Hide();
            _mainMenu = _uiManager.GetScreen<IMainMenu>();
            _mainMenu.Show();
            _timeSystem.PauseTime();
        }

        protected override void OnExit()
        {
            _cameraController.TurnCameraOn();
            _mainMenu.Hide();
        }
    }
}