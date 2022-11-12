#nullable enable
using NovemberProject.System;
using UnityEngine;

namespace NovemberProject.InputSystem
{
    public sealed class ToggleCheatMenuInputHandler:InputHandler
    {
        public override void HandleInput()
        {
            if (!Input.GetKeyDown(InputSystem.TOGGLE_CHEAT_MENU))
            {
                return;
            }

            Game.Instance.UIManager.ToggleCheatMenu();
        }
    }
}