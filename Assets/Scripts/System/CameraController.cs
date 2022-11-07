#nullable enable
using NovemberProject.CommonUIStuff;
using UnityEngine;

namespace NovemberProject.System
{
    public class CameraController : InitializableBehaviour
    {
        private Vector2 _direction;
        private float _zoom;
        private float _zoomDif;

        [SerializeField]
        private Camera _camera = null!;

        [SerializeField]
        private float _speed;

        [SerializeField]
        private float _minHeight;

        [SerializeField]
        private float _maxHeight;

        [SerializeField]
        private float _maxDistance;

        [SerializeField]
        private float _zoomSpeed;

        protected override void Initialize()
        {
            UpdateCameraPosition();
        }

        public void MoveCamera(Vector2 direction)
        {
            _direction += direction;
        }

        public void ZoomCamera(float zoomDif)
        {
            _zoomDif += zoomDif;
        }

        private void LateUpdate()
        {
            if (_direction != Vector2.zero)
            {
                UpdateRigPosition();
            }

            if (_zoomDif != 0)
            {
                UpdateCameraPosition();
            }
        }

        private void UpdateRigPosition()
        {
            _direction = _direction.normalized;
            _direction *= _speed * UnityEngine.Time.deltaTime;
            transform.position += new Vector3(_direction.x, 0, _direction.y);
            _direction = Vector2.zero;
        }

        private void UpdateCameraPosition()
        {
            _zoom += _zoomDif * _zoomSpeed * UnityEngine.Time.deltaTime;
            float cameraHeight = Mathf.Lerp(_maxHeight, _minHeight, _zoom);
            float cameraDistance = Mathf.Lerp(-_maxDistance, 0, _zoom);
            var cameraPosition = new Vector3(0, cameraHeight, cameraDistance);
            var cameraRotation = new Vector3(Mathf.Lerp(60, 0, _zoom), 0, 0);
            _camera.transform.localPosition = cameraPosition;
            _camera.transform.localRotation = Quaternion.Euler(cameraRotation);
            _zoomDif = 0;
        }
    }
}