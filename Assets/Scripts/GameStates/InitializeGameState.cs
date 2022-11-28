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
            Game.Instance.UIManager.HideGameOverPanel();
            Game.Instance.UIManager.HideNewGamePanel();
            Game.Instance.UIManager.HideTechTreePanel();
            Game.Instance.UIManager.HideExpeditionResult();
            Game.Instance.UIManager.HideCreditsScreen();
            Game.Instance.UIManager.HideVictoryScreen();
#if UNITY_EDITOR || DEV_BUILD
            Game.Instance.InputSystem.AddGlobalInputHandler<ToggleCheatMenuInputHandler>();
            Game.Instance.InputSystem.AddGlobalInputHandler<ToggleSystemPanelInputHandler>();
#endif
            Game.Instance.UIManager.HideSystemInfoPanel();
            Game.Instance.UIManager.HideCheatPanel();
            Game.Instance.TimeSystem.PauseTime();
            Game.Instance.UIManager.LockTimeControls();
            Game.Instance.GameStateMachine.MainMenu();
        }

        protected override void OnExit()
        {
        }
    }
}