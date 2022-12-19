#nullable enable
using System;
using NovemberProject.CommonUIStuff;
using NovemberProject.System;
using NovemberProject.Time;
using UniRx;
using UnityEngine;
using Zenject;

namespace NovemberProject.Pops
{
    public sealed class Wobble : InitializableBehaviour
    {
        private const float TOLERANCE = 0.001f;

        private TimeSystem _timeSystem = null!;

        private int _direction = 1;

        [SerializeField]
        private GameObject _model = null!;

        [SerializeField]
        private float _speed;

        [SerializeField]
        private float _maxValue;

        [Inject]
        private void Construct(TimeSystem timeSystem)
        {
            _timeSystem = timeSystem;
        }

        private void Start()
        {
            _timeSystem.OnUpdate.Where(deltaTime => deltaTime != 0).Subscribe(OnUpdate);
        }

        private void OnUpdate(float deltaTime)
        {
            Quaternion quaternion = _model.transform.rotation;
            Vector3 rotation = quaternion.eulerAngles;
            if (rotation.z >= 180)
            {
                rotation.z -= 360f;
            }

            rotation.z += _direction * _speed * deltaTime;
            rotation.z = Mathf.Clamp(rotation.z, -_maxValue, max: _maxValue);
            if (Math.Abs(Mathf.Abs(rotation.z) - _maxValue) < TOLERANCE)
                _direction *= -1;
            if (rotation.z < 0)
            {
                rotation.z += 360f;
            }

            _model.transform.SetPositionAndRotation(_model.transform.position, Quaternion.Euler(rotation));
        }
    }
}