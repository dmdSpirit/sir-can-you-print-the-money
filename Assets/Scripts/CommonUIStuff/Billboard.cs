#nullable enable
using NovemberProject.System;
using UnityEngine;

namespace NovemberProject.CommonUIStuff
{
    public sealed class Billboard : InitializableBehaviour
    {
        private Camera _camera = null!;

        protected override void Initialize()
        {
            base.Initialize();
            _camera = Game.Instance.CameraController.MainCamera;
        }

        private void Update()
        {
            Quaternion cameraRotation = _camera.transform.localRotation;
            float cameraXEulerRotation = cameraRotation.eulerAngles.x;
            Quaternion rotation = transform.localRotation;
            Vector3 eulerRotation = rotation.eulerAngles;
            eulerRotation.x = cameraXEulerRotation;
            // ReSharper disable once Unity.InefficientPropertyAccess
            transform.SetLocalPositionAndRotation(transform.localPosition, Quaternion.Euler(eulerRotation));
        }
    }
}