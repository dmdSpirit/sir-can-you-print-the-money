#nullable enable
using NovemberProject.CommonUIStuff;
using UnityEngine;

namespace NovemberProject.CameraController
{
    public class CameraZoom : InitializableBehaviour
    {
        private float _zoom;
        private float _zoomDif;
        private Camera? _camera;

        [SerializeField]
        private float _minHeight;

        [SerializeField]
        private float _maxHeight;

        [SerializeField]
        private float _maxDistance;

        [SerializeField]
        private float _zoomSpeed;

        [SerializeField]
        private float _keysZoomModifier = .02f;

        public float KeysZoomModifier => _keysZoomModifier;

        public void ZoomCamera(float zoomDif)
        {
            _zoomDif += zoomDif;
        }

        private void LateUpdate()
        {
            if (_zoomDif != 0 && _camera != null)
            {
                UpdateCameraPosition();
            }
        }


        private void UpdateCameraPosition()
        {
            _zoom += _zoomDif * _zoomSpeed * UnityEngine.Time.deltaTime;
            _zoom = Mathf.Clamp(_zoom, 0, 1);
            float cameraHeight = Mathf.Lerp(_maxHeight, _minHeight, _zoom);
            float cameraDistance = Mathf.Lerp(-_maxDistance, 0, _zoom);
            var cameraPosition = new Vector3(0, cameraHeight, cameraDistance);
            var cameraRotation = new Vector3(Mathf.Lerp(60, 0, _zoom), 0, 0);
            _camera!.transform.localPosition = cameraPosition;
            _camera.transform.localRotation = Quaternion.Euler(cameraRotation);
            _zoomDif = 0;
        }

        public void SetCamera(Camera cameraToSet) => _camera = cameraToSet;
    }
}