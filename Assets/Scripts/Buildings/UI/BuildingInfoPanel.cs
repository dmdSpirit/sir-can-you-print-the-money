#nullable enable
using NovemberProject.CommonUIStuff;
using NovemberProject.Core;
using NovemberProject.System;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;
using Zenject;

namespace NovemberProject.Buildings.UI
{
    public interface IBuildingInfoPanel : IUIScreen
    {
        public void SetBuilding(Building building);
    }

    public sealed class BuildingInfoPanel : UIScreen, IBuildingInfoPanel
    {
        private readonly CompositeDisposable _subs = new();

        private Building? _building;

        private CombatController _combatController=null!;

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

        [Inject]
        private void Construct(CombatController combatController)
        {
            _combatController = combatController;
        }


        protected override void OnShow()
        {
            _subs.Clear();
            if (_building == null)
            {
                Debug.LogError($"{nameof(_building)} should be set before showing {nameof(BuildingInfoPanel)}.");
                return;
            }

            _title.text = _building.Title;
            _description.text = _building.Description;
            _image.sprite = _building.Image;
            if (_building is IConstructableBuilding constructableBuilding &&
                constructableBuilding.ConstructableState.Value != ConstructableState.Constructed)
            {
                ShowBuildingConstructionPanel(constructableBuilding);
            }
            else
            {
                _buildingConstructionPanel.Hide();
                ShowWorkerManagement(_building);
                ShowResourceStorage(_building);
                ShowBuyUnit(_building);
                ShowSalaryController(_building);
                ShowTaxController(_building);
                ShowMoneyPrinter(_building);
                ShowMineWorkers(_building);
                ShowIncomingAttack(_building);
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
            _building = null;
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

        public void SetBuilding(Building building)
        {
            _building = building;
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
                if (_combatController.IsActive.Value)
                {
                    _incomingAttack.Show(incomingAttack);
                    return;
                }

                _combatController.IsActive.Subscribe(UpdateIncomingAttack).AddTo(_subs);
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

            Assert.IsTrue(_building is IIncomingAttack);
            var incomingAttack = (IIncomingAttack)_building;
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

            Assert.IsTrue(_building is IExpeditionSender);
            var expeditionSender = (IExpeditionSender)_building;
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