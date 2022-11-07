#nullable enable
using NovemberProject.System;
using UnityEngine;

namespace NovemberProject.InputSystem
{
    public sealed class MoveCameraHandler : InputHandler
    {
        public override void HandleInput()
        {
            Vector2 direction = Vector2.zero;
            if (Input.GetKey(InputSystem.CAMERA_FORWARD_KEY))
            {
                direction.y++;
            }

            if (Input.GetKey(InputSystem.CAMERA_LEFT_KEY))
            {
                direction.x--;
            }

            if (Input.GetKey(InputSystem.CAMERA_BACKWARD_KEY))
            {
                direction.y--;
            }

            if (Input.GetKey(InputSystem.CAMERA_RIGHT_KEY))
            {
                direction.x++;
            }

            Game.Instance.CameraController.MoveCamera(direction.normalized);
            float zoom = Input.GetAxis("Mouse ScrollWheel");
            if (zoom != 0)
            {
                Game.Instance.CameraController.ZoomCamera(zoom);
            }
        }
    }
}