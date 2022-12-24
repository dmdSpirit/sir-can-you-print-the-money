#nullable enable
using NovemberProject.Buildings.UI;
using NovemberProject.Core;
using NovemberProject.TechTree;
using TMPro;
using UniRx;
using UnityEngine;
using Zenject;

namespace NovemberProject.Buildings
{
    public sealed class ArmyTreasuryBuilding : Building, IResourceStorage, ISalaryController
    {
        private MoneyController _moneyController = null!;
        private ArmyManager _armyManager = null!;
        private TechController _techController = null!;

        [SerializeField]
        private TMP_Text _moneyText = null!;

        [SerializeField]
        private Sprite _moneySprite = null!;

        [SerializeField]
        private string _moneyTitle = "Money";

        // public override BuildingType BuildingType => BuildingType.ArmyTreasury;
        public Sprite SpriteIcon => _moneySprite;
        public string ResourceTitle => _moneyTitle;
        public IReadOnlyReactiveProperty<int> ResourceCount => _moneyController.ArmyMoney;
        public IReadOnlyReactiveProperty<bool> CanRaiseSalary => _techController.CanRaiseSalary;
        public IReadOnlyReactiveProperty<bool> CanLowerSalary => _techController.CanLowerSalary;

        [Inject]
        private void Construct(MoneyController moneyController, ArmyManager armyManager, TechController techController)
        {
            _moneyController = moneyController;
            _armyManager = armyManager;
            _techController = techController;
            _moneyController.ArmyMoney.Subscribe(OnMoneyChanged);
        }

        private void OnMoneyChanged(int money)
        {
            _moneyText.text = money.ToString();
        }

        public void RaiseSalary() => _armyManager.RaiseSalary();
        public void LowerSalary() => _armyManager.LowerSalary();
    }
}