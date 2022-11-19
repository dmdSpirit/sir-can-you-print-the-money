#nullable enable
using NovemberProject.InputSystem;
using NovemberProject.System;

namespace NovemberProject.GameStates
{
    public sealed class InitializeGameState : State
    {
        protected override void OnEnter()
        {
            Game.Instance.UIManager.HideEndOfRoundPanel();
            Game.Instance.UIManager.HideRoundStartPanel();
            Game.Instance.UIManager.HideBuildingInfo();
            Game.Instance.UIManager.HideMainMenu();
            Game.Instance.UIManager.HideRoundTimer();
            Game.Instance.UIManager.HideTimeControls();
#if UNITY_EDITOR
            Game.Instance.InputSystem.AddGlobalInputHandler<ToggleCheatMenuInputHandler>();
            Game.Instance.InputSystem.AddGlobalInputHandler<ToggleSystemPanelInputHandler>();
            Game.Instance.UIManager.ShowSystemInfoPanel();
            Game.Instance.UIManager.HideCheatPanel();
#else
            Game.Instance.UIManager.HideSystemInfoPanel();
            Game.Instance.UIManager.HideCheatPanel();
#endif
            Game.Instance.TimeSystem.PauseTime();
            Game.Instance.UIManager.LockTimeControls();
            Game.Instance.GameStateMachine.MainMenu();
        }

        protected override void OnExit()
        {
        }
    }
}