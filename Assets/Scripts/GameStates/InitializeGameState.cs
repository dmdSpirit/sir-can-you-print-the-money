#nullable enable
using NovemberProject.Buildings;
using NovemberProject.Input;
using NovemberProject.System;
using NovemberProject.System.UI;
using NovemberProject.Time;
using NovemberProject.Time.UI;

namespace NovemberProject.GameStates
{
    public sealed class InitializeGameState : State
    {
        private readonly InputSystem _inputSystem;
        private readonly GameStateMachine _gameStateMachine;
        private readonly TimeSystem _timeSystem;
        private readonly UIManager _uiManager;
        private readonly BuildingNameHover _buildingNameHover;

        public InitializeGameState(InputSystem inputSystem, GameStateMachine gameStateMachine, TimeSystem timeSystem,
            UIManager uiManager, BuildingNameHover buildingNameHover)
        {
            _inputSystem = inputSystem;
            _gameStateMachine = gameStateMachine;
            _timeSystem = timeSystem;
            _uiManager = uiManager;
            _buildingNameHover = buildingNameHover;
        }

        protected override void OnEnter()
        {
            _uiManager.HideAll();
            _buildingNameHover.HidePanel();
#if UNITY_EDITOR || DEV_BUILD
            _inputSystem.AddToggleCheatMenuInputHandlerToGlobal(_uiManager);
            _inputSystem.AddToggleSystemPanelInputHandlerToGlobal(_uiManager);
#endif
            _timeSystem.PauseTime();
            var timeControlsPanel = _uiManager.GetScreen<ITimeControlsPanel>();
            timeControlsPanel.Lock();
            _gameStateMachine.MainMenu();
        }

        protected override void OnExit()
        {
        }
    }
}