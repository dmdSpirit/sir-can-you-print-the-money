#nullable enable
using NovemberProject.CommonUIStuff;
using UnityEngine;

namespace NovemberProject.CameraController
{
    [RequireComponent(typeof(CameraMovement))]
    [RequireComponent(typeof(CameraZoom))]
    public class CameraController : InitializableBehaviour
    {
        private CameraMovement _cameraMovement = null!;
        private CameraZoom _cameraZoom = null!;

        public float KeysZoomModifier => _cameraZoom.KeysZoomModifier;
        public float MouseMoveSpeed => _cameraMovement.MouseMoveSpeed;

        private void Awake()
        {
            _cameraMovement = GetComponent<CameraMovement>();
            _cameraZoom = GetComponent<CameraZoom>();
        }

        public void MoveCamera(Vector2 direction) => _cameraMovement.MoveCamera(direction);
        public void ZoomCamera(float zoomDif) => _cameraZoom.ZoomCamera(zoomDif);
        public void SetBounds(Rect bounds) => _cameraMovement.SetBounds(bounds);
    }
}