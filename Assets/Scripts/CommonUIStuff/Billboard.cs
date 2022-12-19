#nullable enable
using NovemberProject.CameraSystem;
using NovemberProject.System;
using UnityEngine;
using Zenject;

namespace NovemberProject.CommonUIStuff
{
    public sealed class Billboard : InitializableBehaviour
    {
        private Transform _cameraTransform = null!;

        [Inject]
        private void Construct(CameraController cameraController)
        {
            _cameraTransform = cameraController.MainCamera.transform;
        }

        private void Update()
        {
            Quaternion cameraRotation = _cameraTransform.localRotation;
            float cameraXEulerRotation = cameraRotation.eulerAngles.x;
            Quaternion rotation = transform.localRotation;
            Vector3 eulerRotation = rotation.eulerAngles;
            eulerRotation.x = cameraXEulerRotation;
            // ReSharper disable once Unity.InefficientPropertyAccess
            transform.SetLocalPositionAndRotation(transform.localPosition, Quaternion.Euler(eulerRotation));
        }
    }
}