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

        [SerializeField]
        private ResourceStoragePanel _resourceStoragePanel = null!;

        [SerializeField]
        private BuyUnitPanel _buyUnitPanel = null!;

        [SerializeField]
        private ExpeditionSenderPanel _expeditionSenderPanel = null!;

        [SerializeField]
        private BuildingConstructionPanel _buildingConstructionPanel = null!;

        public Building Building { get; private set; } = null!;

        protected override void OnShow(Building building)
        {
            Building = building;
            _title.text = Building.Title;
            _description.text = Building.Description;
            _image.sprite = Building.Image;
            if (building is IConstructableBuilding constructableBuilding &&
                constructableBuilding.ConstructableState.Value != ConstructableState.Constructed)
            {
                ShowBuildingConstructionPanel(constructableBuilding);
            }
            else
            {
                _buildingConstructionPanel.Hide();
                ShowWorkerManagement(building);
                ShowResourceStorage(building);
                ShowBuyUnit(building);
            }

            LayoutRebuilder.ForceRebuildLayoutImmediate(transform as RectTransform);
        }

        protected override void OnHide()
        {
            _workerManagementPanel.Hide();
            _resourceStoragePanel.Hide();
            _buyUnitPanel.Hide();
            _expeditionSenderPanel.Hide();
            _buildingConstructionPanel.Hide();
        }

        private void ShowBuyUnit(Building building)
        {
            if (building is IBuyUnit buyUnit)
            {
                _buyUnitPanel.Show(buyUnit);
            }
            else
            {
                _buyUnitPanel.Hide();
            }
        }

        private void ShowResourceStorage(Building building)
        {
            if (building is IResourceStorage resourceStorage)
            {
                _resourceStoragePanel.Show(resourceStorage);
            }
            else
            {
                _resourceStoragePanel.Hide();
            }
        }

        private void ShowWorkerManagement(Building building)
        {
            if (building is IExpeditionSender expeditionSender)
            {
                _expeditionSenderPanel.Show(expeditionSender);
                _workerManagementPanel.Hide();
                return;
            }

            _expeditionSenderPanel.Hide();
            if (building is IWorkerManipulator workerManipulator)
            {
                _workerManagementPanel.Show(workerManipulator);
            }
            else
            {
                _workerManagementPanel.Hide();
            }
        }

        private void ShowBuildingConstructionPanel(IConstructableBuilding constructableBuilding)
        {
            _workerManagementPanel.Hide();
            _resourceStoragePanel.Hide();
            _buyUnitPanel.Hide();
            _expeditionSenderPanel.Hide();
            _buildingConstructionPanel.Show(constructableBuilding);
        }
    }
}