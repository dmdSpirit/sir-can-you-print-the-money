#nullable enable
using NovemberProject.CameraSystem;
using NovemberProject.System;
using NovemberProject.System.UI;
using UnityEngine;

namespace NovemberProject.Input
{
    public sealed class MoveCameraHandler : InputHandler
    {
        private const int RIGHT_MOUSE_BUTTON = 1;

        private readonly CameraController _cameraController;
        private readonly UIManager _uiManager;

        public MoveCameraHandler(CameraController cameraController, UIManager uiManager)
        {
            _cameraController = cameraController;
            _uiManager = uiManager;
        }

        public override void HandleInput()
        {
            Vector2 direction = GetRigMovementDirection();
            if (direction != Vector2.zero)
            {
                _cameraController.MoveCamera(direction);
            }

            float zoom = GetZoom();
            if (zoom != 0)
            {
                _cameraController.ZoomCamera(zoom);
            }
        }

        private Vector2 GetRigMovementDirection()
        {
            Vector2 direction = GetKeyboardMovement();
            if (!UnityEngine.Input.GetMouseButton(RIGHT_MOUSE_BUTTON) || _uiManager.IsMouseOver.Value)
            {
                return direction.normalized;
            }

            float mouseMoveSpeed = _cameraController.MouseMoveSpeed;
            direction.x -= UnityEngine.Input.GetAxis("Mouse X") * mouseMoveSpeed;
            direction.y -= UnityEngine.Input.GetAxis("Mouse Y") * mouseMoveSpeed;

            return direction.normalized;
        }

        private static Vector2 GetKeyboardMovement()
        {
            Vector2 direction = Vector2.zero;
            if (UnityEngine.Input.GetKey(InputKeys.CAMERA_FORWARD_KEY))
            {
                direction.y++;
            }

            if (UnityEngine.Input.GetKey(InputKeys.CAMERA_LEFT_KEY))
            {
                direction.x--;
            }

            if (UnityEngine.Input.GetKey(InputKeys.CAMERA_BACKWARD_KEY))
            {
                direction.y--;
            }

            if (UnityEngine.Input.GetKey(InputKeys.CAMERA_RIGHT_KEY))
            {
                direction.x++;
            }

            return direction;
        }

        private float GetZoom()
        {
            float zoom = UnityEngine.Input.GetAxis("Mouse ScrollWheel");
            if (UnityEngine.Input.GetKey(InputKeys.CAMERA_ZOOM_IN_KEY))
            {
                zoom += _cameraController.KeysZoomModifier;
            }

            if (UnityEngine.Input.GetKey(InputKeys.CAMERA_ZOOM_OUT_KEY))
            {
                zoom -= _cameraController.KeysZoomModifier;
            }

            return Mathf.Clamp(zoom, -1, 1);
        }
    }
}