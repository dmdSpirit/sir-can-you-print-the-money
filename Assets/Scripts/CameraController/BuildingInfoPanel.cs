#nullable enable
using NovemberProject.CommonUIStuff;
using NovemberProject.System;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace NovemberProject.CameraController
{
    public class BuildingInfoPanel : UIElement<Building>
    {
        private Building _building = null!;

        [SerializeField]
        private TMP_Text _title = null!;

        [SerializeField]
        private TMP_Text _description = null!;

        [SerializeField]
        private Image _image = null!;

        [SerializeField]
        private Button _closeButton = null!;

        public Building Building => _building;

        protected override void Initialize()
        {
            base.Initialize();
            _closeButton.OnClickAsObservable()
                .TakeUntilDisable(this)
                .Subscribe(OnCloseButtonClicked);
        }

        protected override void OnShow(Building building)
        {
            _building = building;
            _title.text = _building.Title;
            _description.text = _building.Description;
            _image.sprite = _building.Image;
        }

        protected override void OnHide()
        {
        }

        private void OnCloseButtonClicked(Unit _)
        {
            Assert.IsTrue(IsShown);
            Game.Instance.UIManager.HideBuildingInfo();
        }
    }
}