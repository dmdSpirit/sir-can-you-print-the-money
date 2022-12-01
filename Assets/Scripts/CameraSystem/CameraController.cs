#nullable enable
using DG.Tweening;
using NovemberProject.CommonUIStuff;
using NovemberProject.System;
using NovemberProject.System.Messages;
using UniRx;
using UnityEngine;
using NotImplementedException = System.NotImplementedException;

namespace NovemberProject.CameraSystem
{
    [RequireComponent(typeof(CameraMovement))]
    [RequireComponent(typeof(CameraZoom))]
    public sealed class CameraController : InitializableBehaviour
    {
        private Vector3 _initialPosition;
        private CameraMovement _cameraMovement = null!;
        private CameraZoom _cameraZoom = null!;

        [SerializeField]
        private Camera _mainCamera = null!;

        [SerializeField, Range(0, 1)]
        private float _startingZoom = .6f;

        public float KeysZoomModifier => _cameraZoom.KeysZoomModifier;
        public float MouseMoveSpeed => _cameraMovement.MouseMoveSpeed;
        public Camera MainCamera => _mainCamera;

        private void Awake()
        {
            _cameraMovement = GetComponent<CameraMovement>();
            _cameraZoom = GetComponent<CameraZoom>();
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            _cameraZoom.SetCamera(_mainCamera);
            _initialPosition = transform.position;
            Game.Instance.MessageBroker.Receive<NewGameMessage>()
                .TakeUntilDisable(this)
                .Subscribe(ResetPosition);
        }

        private void ResetPosition(NewGameMessage _)
        {
            transform.position = _initialPosition;
        }

        public void MoveCamera(Vector2 direction) => _cameraMovement.MoveCamera(direction);
        public void ZoomCamera(float zoomDif) => _cameraZoom.ZoomCamera(zoomDif);
        public void SetBounds(Rect bounds) => _cameraMovement.SetBounds(bounds);

        public void InitializeGameData()
        {
            _cameraZoom.SetCameraZoom(_startingZoom);
        }

        public void TurnCameraOff()
        {
            _mainCamera.enabled = false;
        }
        
        public void TurnCameraOn()
        {
            _mainCamera.enabled = true;
        }

        public Tween MoveTo(Transform cameraPosition,float cameraMoveDuration, float cameraZoom)
        {
            _cameraZoom.TweenZoom(cameraZoom, cameraMoveDuration);
            return transform.DOMove(cameraPosition.position, cameraMoveDuration);
        }
    }
}