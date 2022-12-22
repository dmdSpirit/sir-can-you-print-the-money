#nullable enable
using NovemberProject.System.UI;

namespace NovemberProject.Input
{
    public sealed class ToggleSystemPanelInputHandler : InputHandler
    {
        private readonly UIManager _uiManager;

        public ToggleSystemPanelInputHandler(UIManager uiManager)
        {
            _uiManager = uiManager;
        }

        public override void HandleInput()
        {
            if (!UnityEngine.Input.GetKeyDown(InputKeys.TOGGLE_SYSTEM_PANEL))
            {
                return;
            }

            var systemInfoPanel = _uiManager.GetScreen<ISystemInfoPanel>();
            if (systemInfoPanel.IsShown)
            {
                systemInfoPanel.Hide();
            }
            else
            {
                systemInfoPanel.Show();
            }
        }
    }
}