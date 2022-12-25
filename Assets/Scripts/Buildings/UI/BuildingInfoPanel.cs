#nullable enable
using NovemberProject.CommonUIStuff;
using NovemberProject.Core;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;
using Zenject;

namespace NovemberProject.Buildings.UI
{
    public sealed class BuildingInfoPanel : UIScreen, IBuildingInfoPanel
    {
        private readonly CompositeDisposable _subs = new();

        private Building _building = null!;

        private CombatController _combatController = null!;

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

            FillBuildingInfo(_building);
            var constructableBuilding = _building.GetBuildingFunction<IConstructableBuilding>();
            if (IsContracted(constructableBuilding))
            {
                ShowConstructedBuildingPanels();
            }
            else
            {
                ShowBuildingConstructionPanel(constructableBuilding);
            }

            LayoutRebuilder.ForceRebuildLayoutImmediate(transform as RectTransform);

            bool IsContracted(IConstructableBuilding? constructable) => constructable == null ||
                                                                        constructable.ConstructableState.Value ==
                                                                        ConstructableState.Constructed;
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
            _building = null!;
        }

        public void SetBuilding(Building building)
        {
            _building = building;
        }

        private void ShowConstructedBuildingPanels()
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

        private void ShowBuyUnit(Building building)
        {
            var unitBuyer = building.GetBuildingFunction<IBuyUnit>();
            if (unitBuyer == null)
            {
                _buyUnitPanel.Hide();
                return;
            }

            _buyUnitPanel.Show(unitBuyer);
        }

        private void FillBuildingInfo(IBuildingInfo buildingInfo)
        {
            _title.text = buildingInfo.Title;
            _description.text = buildingInfo.Description;
            _image.sprite = buildingInfo.Image;
        }

        private void ShowSalaryController(Building building)
        {
            var salaryController = building.GetBuildingFunction<ISalaryController>();
            if (salaryController == null)
            {
                _salaryControlPanel.Hide();
                return;
            }

            _salaryControlPanel.Show(salaryController);
        }

        private void ShowTaxController(Building building)
        {
            var taxController = building.GetBuildingFunction<ITaxController>();
            if (taxController == null)
            {
                _taxControlPanel.Hide();
                return;
            }

            _taxControlPanel.Show(taxController);
        }

        private void ShowMoneyPrinter(Building building)
        {
            var moneyPrinter = building.GetBuildingFunction<IMoneyPrinter>();
            if (moneyPrinter == null)
            {
                _moneyPrinterPanel.Hide();
                return;
            }

            _moneyPrinterPanel.Show(moneyPrinter);
        }

        private void ShowResourceStorage(Building building)
        {
            var resourceStorage = building.GetBuildingFunction<IResourceStorage>();
            if (resourceStorage == null)
            {
                _resourceStoragePanel.Hide();
                return;
            }

            _resourceStoragePanel.Show(resourceStorage);
        }

        private void ShowMineWorkers(Building building)
        {
            var mineWorkerManipulator = building.GetBuildingFunction<IMineWorkerManipulator>();
            if (mineWorkerManipulator == null)
            {
                _mineWorkerManagementPanel.Hide();
                return;
            }

            _mineWorkerManagementPanel.Show(mineWorkerManipulator);
        }

        private void ShowIncomingAttack(Building building)
        {
            var incomingAttack = building.GetBuildingFunction<IIncomingAttack>();
            if (incomingAttack == null)
            {
                _incomingAttack.Hide();
                return;
            }

            if (_combatController.IsActive.Value)
            {
                _incomingAttack.Show(incomingAttack);
                return;
            }

            _combatController.IsActive.Subscribe(UpdateIncomingAttack).AddTo(_subs);
        }

        // ReSharper disable once FlagArgument
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
            var incomingAttack = (IIncomingAttack)_building!;
            _incomingAttack.Show(incomingAttack);
        }

        private void ShowWorkerManagement(Building building)
        {
            var expeditionSender = building.GetBuildingFunction<IExpeditionSender>();
            if (expeditionSender != null)
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
            var workerManipulator = building.GetBuildingFunction<IWorkerManipulator>();
            if (workerManipulator == null)
            {
                _workerManagementPanel.Hide();
                return;
            }

            _workerManagementPanel.Show(workerManipulator);
        }

        // ReSharper disable once FlagArgument
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
            var expeditionSender = (IExpeditionSender)_building!;
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