#nullable enable
using NovemberProject.CommonUIStuff;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace NovemberProject.Buildings.UI
{
    public sealed class BuildingInfoPanel : UIElement<Building>
    {
        [SerializeField]
        private TMP_Text _title = null!;

        [SerializeField]
        private TMP_Text _description = null!;

        [SerializeField]
        private Image _image = null!;

        [SerializeField]
        private WorkerManagementPanel _workerManagementPanel = null!;

        public Building Building { get; private set; } = null!;

        protected override void OnShow(Building building)
        {
            Building = building;
            _title.text = Building.Title;
            _description.text = Building.Description;
            _image.sprite = Building.Image;
            if (building is IWorkerManipulator workerManipulator)
            {
                _workerManagementPanel.Show(workerManipulator);
            }
            else
            {
                _workerManagementPanel.Hide();
            }

            LayoutRebuilder.ForceRebuildLayoutImmediate(transform as RectTransform);
        }

        protected override void OnHide()
        {
            _workerManagementPanel.Hide();
        }
    }
}