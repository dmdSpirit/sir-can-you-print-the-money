#nullable enable
using NovemberProject.System;

namespace NovemberProject.Input
{
    public sealed class ToggleSystemPanelInputHandler : InputHandler
    {
        public override void HandleInput()
        {
            if (!UnityEngine.Input.GetKeyDown(InputKeys.TOGGLE_SYSTEM_PANEL))
            {
                return;
            }

            Game.Instance.UIManager.ToggleSystemInfoPanel();
        }
    }
}