#nullable enable
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

        protected override void Initialize()
        {
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

        private float GetMinX()
        {
            var minX = float.MaxValue;
            foreach (GameObject bound in _bounds)
            {
                if (bound.transform.position.x < minX)
                {
                    minX = bound.transform.position.x;
                }
            }

            return minX;
        }

        private float GetMaxX()
        {
            var maxX = float.MinValue;
            foreach (GameObject bound in _bounds)
            {
                if (bound.transform.position.x > maxX)
                {
                    maxX = bound.transform.position.x;
                }
            }

            return maxX;
        }

        private float GetMinY()
        {
            var minY = float.MaxValue;
            foreach (GameObject bound in _bounds)
            {
                if (bound.transform.position.z < minY)
                {
                    minY = bound.transform.position.z;
                }
            }

            return minY;
        }

        private float GetMaxY()
        {
            var maxY = float.MinValue;
            foreach (GameObject bound in _bounds)
            {
                if (bound.transform.position.z > maxY)
                {
                    maxY = bound.transform.position.z;
                }
            }

            return maxY;
        }
    }
}