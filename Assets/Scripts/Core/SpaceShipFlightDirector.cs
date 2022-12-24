#nullable enable
using System;
using DG.Tweening;
using NovemberProject.Buildings;
using NovemberProject.CameraSystem;
using NovemberProject.CommonUIStuff;
using UniRx;
using UnityEngine;
using Zenject;

namespace NovemberProject.Core
{
    public sealed class SpaceShipFlightDirector : InitializableBehaviour
    {
        private readonly Subject<Unit> _onFinishedPlaying = new();

        private BuildingsController _buildingsController = null!;
        private CameraController _cameraController = null!;

        [SerializeField]
        private Transform _cameraPosition = null!;

        [SerializeField]
        private float _cameraZoom = 0.8f;

        [SerializeField]
        private Transform _castlePosition = null!;

        [SerializeField]
        private float _cameraMoveDuration = 2f;

        [SerializeField]
        private float _castleFlightDuration = 12f;

        [SerializeField]
        private float _cameraFlightDuration = 6f;

        public IObservable<Unit> OnFinishedPlaying => _onFinishedPlaying;

        [Inject]
        private void Construct(BuildingsController buildingsController, CameraController cameraController)
        {
            _buildingsController = buildingsController;
            _cameraController = cameraController;
        }

        public void StartSequence()
        {
            Tween cameraTween =
                _cameraController.MoveTo(_cameraPosition, _cameraMoveDuration, _cameraZoom);
            cameraTween.onComplete += OnMoveFinished;
        }

        private void OnMoveFinished()
        {
            Building arena = _buildingsController.GetBuilding(BuildingType.Arena);

            var tween = arena.transform.DOMove(_castlePosition.position, _castleFlightDuration);
            tween.SetEase(Ease.InQuad);
            tween.onComplete += () => _onFinishedPlaying.OnNext(Unit.Default);
            var cameraPos = _cameraController.transform.position + new Vector3(0, 3, 0);
            var cameraTween = _cameraController.transform.DOMove(cameraPos, _cameraFlightDuration).SetDelay(1f);
            cameraTween.SetEase(Ease.InQuad);
        }
    }
}