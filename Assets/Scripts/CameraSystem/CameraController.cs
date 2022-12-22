#nullable enable
using System;
using DG.Tweening;
using NovemberProject.CommonUIStuff;
using NovemberProject.System.Messages;
using UniRx;
using UnityEngine;
using Zenject;

namespace NovemberProject.CameraSystem
{
    [RequireComponent(typeof(CameraMovement))]
    [RequireComponent(typeof(CameraZoom))]
    public sealed class CameraController : InitializableBehaviour
    {
        private MessageBroker _messageBroker = null!;
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

        [Inject]
        private void Construct(MessageBroker messageBroker)
        {
            _messageBroker = messageBroker;
            _messageBroker.Receive<NewGameMessage>().Subscribe(OnNewGame);
            _cameraMovement = GetComponent<CameraMovement>();
            _cameraZoom = GetComponent<CameraZoom>();
        }

        private void Start()
        {
            _cameraZoom.SetCamera(_mainCamera);
            _initialPosition = transform.position;
        }

        private void OnNewGame(NewGameMessage message)
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

        public Tween MoveTo(Transform cameraPosition, float cameraMoveDuration, float cameraZoom)
        {
            _cameraZoom.TweenZoom(cameraZoom, cameraMoveDuration);
            return transform.DOMove(cameraPosition.position, cameraMoveDuration);
        }
    }
}