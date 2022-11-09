#nullable enable
using NovemberProject.CommonUIStuff;
using NovemberProject.System;
using UnityEngine;

namespace NovemberProject.CameraSystem
{
    public class Billboard : InitializableBehaviour
    {
        private Camera _camera = null!;

        protected override void Initialize()
        {
            base.Initialize();
            _camera = Game.Instance.CameraController.MainCamera;
        }

        private void Update()
        {
            float cameraRotation = _camera.transform.localRotation.eulerAngles.x;
            Vector3 rotation = transform.localRotation.eulerAngles;
            rotation.x = cameraRotation;
            transform.SetLocalPositionAndRotation(transform.localPosition, Quaternion.Euler(rotation));
        }
    }
}