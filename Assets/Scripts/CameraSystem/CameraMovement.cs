#nullable enable
using NovemberProject.CommonUIStuff;
using UnityEngine;

namespace NovemberProject.CameraSystem
{
    public class CameraMovement : InitializableBehaviour
    {
        private Vector2 _direction;
        private bool _areBoundsSet;
        private Rect _bounds;

        [SerializeField]
        private float _speed;

        [SerializeField]
        private float _mouseMoveSpeed = .5f;

        public float MouseMoveSpeed => _mouseMoveSpeed;

        public void MoveCamera(Vector2 direction)
        {
            _direction += direction;
        }

        private void LateUpdate()
        {
            if (_direction != Vector2.zero)
            {
                UpdateRigPosition();
            }
        }

        private void UpdateRigPosition()
        {
            _direction = _direction.normalized;
            _direction *= _speed * UnityEngine.Time.deltaTime;

            Vector3 position = transform.position + new Vector3(_direction.x, 0, _direction.y);
            if (_areBoundsSet)
            {
                position.x = Mathf.Clamp(position.x, _bounds.xMin, _bounds.xMax);
                position.z = Mathf.Clamp(position.z, _bounds.yMin, _bounds.yMax);
            }

            transform.position = position;
            _direction = Vector2.zero;
        }

        public void SetBounds(Rect bounds)
        {
            _areBoundsSet = true;
            _bounds = bounds;
        }
    }
}