#nullable enable
using NovemberProject.System;
using UnityEngine;

namespace NovemberProject.InputSystem
{
    public sealed class ToggleSystemPanelInputHandler : InputHandler
    {
        public override void HandleInput()
        {
            if (!Input.GetKeyDown(InputKeys.TOGGLE_SYSTEM_PANEL))
            {
                return;
            }

            Game.Instance.UIManager.ToggleSystemInfoPanel();
        }
    }
}