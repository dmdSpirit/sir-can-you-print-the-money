#nullable enable
using System;
using NovemberProject.CommonUIStuff;
using UniRx;
using UnityEngine.EventSystems;

namespace NovemberProject.CameraController
{
    public class BuildingClickObserver : InitializableBehaviour, IPointerClickHandler
    {
        private readonly Subject<Unit> _onBuildingClicked = new();

        public IObservable<Unit> OnBuildingClicked => _onBuildingClicked;

        public void OnPointerClick(PointerEventData eventData)
        {
            _onBuildingClicked.OnNext(Unit.Default);
        }
    }
}