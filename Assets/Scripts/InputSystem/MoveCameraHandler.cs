#nullable enable
using NovemberProject.CameraSystem;
using NovemberProject.System;
using UnityEngine;

namespace NovemberProject.InputSystem
{
    public sealed class MoveCameraHandler : InputHandler
    {
        private const int RIGHT_MOUSE_BUTTON = 1;

        public override void HandleInput()
        {
            Vector2 direction = GetRigMovementDirection();
            if (direction != Vector2.zero)
            {
                Game.Instance.CameraController.MoveCamera(direction);
            }

            float zoom = GetZoom();
            if (zoom != 0)
            {
                Game.Instance.CameraController.ZoomCamera(zoom);
            }
        }

        private Vector2 GetRigMovementDirection()
        {
            Vector2 direction = GetKeyboardMovement();

            if (!Input.GetMouseButton(RIGHT_MOUSE_BUTTON))
            {
                return direction.normalized;
            }

            CameraController cameraController = Game.Instance.CameraController;
            float mouseMoveSpeed = cameraController.MouseMoveSpeed;
            direction.x -= Input.GetAxis("Mouse X") * mouseMoveSpeed;
            direction.y -= Input.GetAxis("Mouse Y") * mouseMoveSpeed;

            return direction.normalized;
        }

        private Vector2 GetKeyboardMovement()
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

            return direction;
        }

        private float GetZoom()
        {
            float zoom = Input.GetAxis("Mouse ScrollWheel");
            if (Input.GetKey(InputSystem.CAMERA_ZOOM_IN_KEY))
            {
                zoom += Game.Instance.CameraController.KeysZoomModifier;
            }

            if (Input.GetKey(InputSystem.CAMERA_ZOOM_OUT_KEY))
            {
                zoom -= Game.Instance.CameraController.KeysZoomModifier;
            }

            return Mathf.Clamp(zoom, -1, 1);
        }
    }
}