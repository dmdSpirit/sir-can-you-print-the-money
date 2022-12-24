#nullable enable
using System.Linq;
using UnityEngine;
using Zenject;

namespace NovemberProject.CameraSystem
{
    public sealed class CameraBounds : MonoBehaviour
    {
        private const int VALID_NUMBER_OF_BOUNDS = 4;
        private CameraController _cameraController = null!;

        [SerializeField]
        private GameObject[] _bounds = null!;

        [Inject]
        private void Construct(CameraController cameraController)
        {
            _cameraController = cameraController;
        }

        private void Start()
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
            _cameraController.SetBounds(rect);
        }

        private float GetMinX() => _bounds.Select(bound => bound.transform.position.x).Min();
        private float GetMaxX() => _bounds.Select(bound => bound.transform.position.x).Max();
        private float GetMinY() => _bounds.Select(bound => bound.transform.position.z).Min();
        private float GetMaxY() => _bounds.Select(bound => bound.transform.position.z).Max();
    }
}