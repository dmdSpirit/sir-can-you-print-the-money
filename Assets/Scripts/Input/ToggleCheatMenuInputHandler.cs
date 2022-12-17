#nullable enable
using NovemberProject.System;

namespace NovemberProject.Input
{
    public sealed class ToggleCheatMenuInputHandler : InputHandler
    {
        public override void HandleInput()
        {
            if (!UnityEngine.Input.GetKeyDown(InputKeys.TOGGLE_CHEAT_MENU))
            {
                return;
            }

            Game.Instance.UIManager.ToggleCheatMenu();
        }
    }
}