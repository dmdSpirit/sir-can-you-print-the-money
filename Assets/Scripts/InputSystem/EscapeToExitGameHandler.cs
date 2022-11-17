#nullable enable
using NovemberProject.System;
using UnityEngine;

namespace NovemberProject.InputSystem
{
    public sealed class EscapeToExitGameHandler : InputHandler
    {
        public override void HandleInput()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Game.Instance.GameStateMachine.ExitGame();
            }
        }
    }
}