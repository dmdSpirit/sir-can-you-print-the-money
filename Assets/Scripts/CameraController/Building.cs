#nullable enable
using NovemberProject.CommonUIStuff;
using NovemberProject.System;
using UniRx;
using UnityEngine;

namespace NovemberProject.CameraController
{
    public class Building : InitializableBehaviour
    {
        [SerializeField]
        private BuildingClickObserver _clickObserver = null!;

        [SerializeField]
        private string _title = null!;

        [SerializeField]
        private string _description = null!;

        [SerializeField]
        private Sprite _image = null!;

        public string Title => _title;
        public string Description => _description;
        public Sprite Image => _image;

        protected override void Initialize()
        {
            base.Initialize();
            _clickObserver.OnBuildingClicked
                .TakeUntilDisable(this)
                .Subscribe(OnBuildingClick);
        }

        private void OnBuildingClick(Unit _)
        {
            Game.Instance.UIManager.ShowBuildingInfo(this);
        }
    }
}