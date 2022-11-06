#nullable enable
using NovemberProject.System;
using UnityEngine;

namespace NovemberProject.Input
{
    public sealed class EscapeToExitGameHandler : InputHandler
    {
        public override void HandleInput()
        {
            if (UnityEngine.Input.GetKeyDown(KeyCode.Escape))
            {
                Game.Instance.ExitGame();
            }
        }
    }
}