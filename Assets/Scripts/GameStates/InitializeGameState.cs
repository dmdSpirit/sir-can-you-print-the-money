#nullable enable
using NovemberProject.Input;
using NovemberProject.System;
using NovemberProject.Time;

namespace NovemberProject.GameStates
{
    public sealed class InitializeGameState : State
    {
        private readonly InputSystem _inputSystem;
        private readonly GameStateMachine _gameStateMachine;
        private readonly TimeSystem _timeSystem;

        public InitializeGameState(InputSystem inputSystem, GameStateMachine gameStateMachine, TimeSystem timeSystem)
        {
            _inputSystem = inputSystem;
            _gameStateMachine = gameStateMachine;
            _timeSystem = timeSystem;
        }

        protected override void OnEnter()
        {
            Game.Instance.UIManager.HideEndOfRoundPanel();
            Game.Instance.UIManager.HideRoundStartPanel();
            Game.Instance.UIManager.HideBuildingInfo();
            Game.Instance.UIManager.HideMainMenu();
            Game.Instance.UIManager.HideRoundTimer();
            Game.Instance.UIManager.HideTimeControls();
            Game.Instance.UIManager.HideGameOverPanel();
            Game.Instance.UIManager.HideTutorialScreen();
            Game.Instance.UIManager.HideTechTreeScreen();
            Game.Instance.UIManager.HideExpeditionResult();
            Game.Instance.UIManager.HideCreditsScreen();
            Game.Instance.UIManager.HideVictoryScreen();
            Game.Instance.UIManager.HideAttackResultsPanel();
            Game.Instance.UIManager.HideNotificationsPanel();
            Game.Instance.BuildingNameHover.HidePanel();
#if UNITY_EDITOR || DEV_BUILD
            _inputSystem.AddGlobalInputHandler<ToggleCheatMenuInputHandler>();
            _inputSystem.AddGlobalInputHandler<ToggleSystemPanelInputHandler>();
#endif
            Game.Instance.UIManager.HideSystemInfoPanel();
            Game.Instance.UIManager.HideCheatPanel();
            _timeSystem.PauseTime();
            Game.Instance.UIManager.LockTimeControls();
            _gameStateMachine.MainMenu();
        }

        protected override void OnExit()
        {
        }
    }
}