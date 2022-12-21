#nullable enable
using NovemberProject.Cheats;
using NovemberProject.System;
using NovemberProject.System.UI;

namespace NovemberProject.Input
{
    public sealed class ToggleCheatMenuInputHandler : InputHandler
    {
        private readonly UIManager _uiManager;

        public ToggleCheatMenuInputHandler(UIManager uiManager)
        {
            _uiManager = uiManager;
        }

        public override void HandleInput()
        {
            if (!UnityEngine.Input.GetKeyDown(InputKeys.TOGGLE_CHEAT_MENU))
            {
                return;
            }

            var cheatMenu = _uiManager.GetScreen<ICheatMenu>();
            if (cheatMenu.IsShown)
            {
                cheatMenu.Hide();
            }
            else
            {
                cheatMenu.Show();
            }
        }
    }
}