#nullable enable
using NovemberProject.CommonUIStuff;
using NovemberProject.System;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace NovemberProject.Buildings.UI
{
    public sealed class BuildingInfoPanel : UIElement<Building>
    {
        private readonly CompositeDisposable _subs = new();

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
        private SalaryControlPanel _salaryControlPanel = null!;

        [SerializeField]
        private TaxControlPanel _taxControlPanel = null!;

        [SerializeField]
        private MoneyPrinterPanel _moneyPrinterPanel = null!;

        [SerializeField]
        private MineWorkerManagementPanel _mineWorkerManagementPanel = null!;

        [SerializeField]
        private BuildingConstructionPanel _buildingConstructionPanel = null!;

        [SerializeField]
        private IncomingAttackPanel _incomingAttack = null!;

        public Building Building { get; private set; } = null!;

        protected override void OnShow(Building building)
        {
            _subs.Clear();
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
                ShowSalaryController(building);
                ShowTaxController(building);
                ShowMoneyPrinter(building);
                ShowMineWorkers(building);
                ShowIncomingAttack(building);
            }

            LayoutRebuilder.ForceRebuildLayoutImmediate(transform as RectTransform);
        }

        protected override void OnHide()
        {
            _subs.Clear();
            _workerManagementPanel.Hide();
            _resourceStoragePanel.Hide();
            _buyUnitPanel.Hide();
            _expeditionSenderPanel.Hide();
            _buildingConstructionPanel.Hide();
            _salaryControlPanel.Hide();
            _taxControlPanel.Hide();
            _moneyPrinterPanel.Hide();
            _mineWorkerManagementPanel.Hide();
            _incomingAttack.Hide();
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

        private void ShowSalaryController(Building building)
        {
            if (building is ISalaryController salaryController)
            {
                _salaryControlPanel.Show(salaryController);
            }
            else
            {
                _salaryControlPanel.Hide();
            }
        }

        private void ShowTaxController(Building building)
        {
            if (building is ITaxController taxController)
            {
                _taxControlPanel.Show(taxController);
            }
            else
            {
                _taxControlPanel.Hide();
            }
        }

        private void ShowMoneyPrinter(Building building)
        {
            if (building is IMoneyPrinter moneyPrinter)
            {
                _moneyPrinterPanel.Show(moneyPrinter);
            }
            else
            {
                _moneyPrinterPanel.Hide();
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

        private void ShowMineWorkers(Building building)
        {
            if (building is IMineWorkerManipulator mineWorkerManipulator)
            {
                _mineWorkerManagementPanel.Show(mineWorkerManipulator);
            }
            else
            {
                _mineWorkerManagementPanel.Hide();
            }
        }

        private void ShowIncomingAttack(Building building)
        {
            if (building is IIncomingAttack incomingAttack)
            {
                if (Game.Instance.CombatController.IsActive.Value)
                {
                    _incomingAttack.Show(incomingAttack);
                    return;
                }

                Game.Instance.CombatController.IsActive.Subscribe(UpdateIncomingAttack).AddTo(_subs);
            }
            else
            {
                _incomingAttack.Hide();
            }
        }

        private void UpdateIncomingAttack(bool isActive)
        {
            if (!isActive)
            {
                _incomingAttack.Hide();
                return;
            }

            if (_incomingAttack.IsShown)
            {
                return;
            }

            Assert.IsTrue(Building is IIncomingAttack);
            var incomingAttack = (IIncomingAttack)Building;
            _incomingAttack.Show(incomingAttack);
            
        }

        private void ShowWorkerManagement(Building building)
        {
            if (building is IExpeditionSender expeditionSender)
            {
                _workerManagementPanel.Hide();
                if (expeditionSender.IsActive.Value)
                {
                    _expeditionSenderPanel.Show(expeditionSender);
                }
                else
                {
                    _expeditionSenderPanel.Hide();
                    expeditionSender.IsActive.Subscribe(UpdateExpeditionSender).AddTo(_subs);
                }

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

        private void UpdateExpeditionSender(bool isActive)
        {
            if (!isActive)
            {
                _expeditionSenderPanel.Hide();
                return;
            }

            if (_expeditionSenderPanel.IsShown)
            {
                return;
            }

            Assert.IsTrue(Building is IExpeditionSender);
            var expeditionSender = (IExpeditionSender)Building;
            _expeditionSenderPanel.Show(expeditionSender);
        }

        private void ShowBuildingConstructionPanel(IConstructableBuilding constructableBuilding)
        {
            _workerManagementPanel.Hide();
            _resourceStoragePanel.Hide();
            _buyUnitPanel.Hide();
            _expeditionSenderPanel.Hide();
            _salaryControlPanel.Hide();
            _taxControlPanel.Hide();
            _moneyPrinterPanel.Hide();
            _mineWorkerManagementPanel.Hide();
            _incomingAttack.Hide();
            _buildingConstructionPanel.Show(constructableBuilding);
        }
    }
}