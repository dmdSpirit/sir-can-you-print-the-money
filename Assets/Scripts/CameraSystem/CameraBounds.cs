#nullable enable
using System.Linq;
using NovemberProject.CommonUIStuff;
using NovemberProject.System;
using UnityEngine;

namespace NovemberProject.CameraSystem
{
    public sealed class CameraBounds : InitializableBehaviour
    {
        private const int VALID_NUMBER_OF_BOUNDS = 4;

        [SerializeField]
        private GameObject[] _bounds = null!;

        protected override void OnInitialized()
        {
            base.OnInitialized();
            if (_bounds.Length != VALID_NUMBER_OF_BOUNDS)
            {
                return;
            }

            float minX = GetMinX();
            float width = GetMaxX() - minX;
            float minY = GetMinY();
            float height = GetMaxY() - minY;
            var rect = new Rect(minX, minY, width, height);
            Game.Instance.CameraController.SetBounds(rect);
        }

        private float GetMinX() => _bounds.Select(bound => bound.transform.position.x).Min();
        private float GetMaxX() => _bounds.Select(bound => bound.transform.position.x).Max();
        private float GetMinY() => _bounds.Select(bound => bound.transform.position.z).Min();
        private float GetMaxY() => _bounds.Select(bound => bound.transform.position.z).Max();
    }
}