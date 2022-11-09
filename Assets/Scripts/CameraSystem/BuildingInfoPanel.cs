#nullable enable
using NovemberProject.CommonUIStuff;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace NovemberProject.CameraSystem
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

        public Building Building => _building;

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
    }
}