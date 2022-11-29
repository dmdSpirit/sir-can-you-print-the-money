#nullable enable
using NovemberProject.Buildings.UI;
using NovemberProject.CoreGameplay;
using NovemberProject.System;
using TMPro;
using UniRx;
using UnityEngine;

namespace NovemberProject.Buildings
{
    public sealed class GovernmentTreasuryBuilding : Building, IResourceStorage, ISalaryController, ITaxController
    {
        private MoneyController _moneyController = null!;

        [SerializeField]
        private TMP_Text _moneyText = null!;

        [SerializeField]
        private Sprite _moneySprite = null!;

        [SerializeField]
        private string _moneyTitle = "Money";

        public override BuildingType BuildingType => BuildingType.GovernmentTreasury;
        public Sprite SpriteIcon => _moneySprite;
        public string ResourceTitle => _moneyTitle;
        public IReadOnlyReactiveProperty<int> ResourceCount => Game.Instance.MoneyController.GovernmentMoney;
        public IReadOnlyReactiveProperty<bool> CanRaiseSalary => Game.Instance.TechController.CanRaiseSalary;
        public IReadOnlyReactiveProperty<bool> CanLowerSalary => Game.Instance.TechController.CanLowerSalary;
        public IReadOnlyReactiveProperty<bool> CanRaiseTax => Game.Instance.TechController.CanRaiseTax;
        public IReadOnlyReactiveProperty<bool> CanLowerTax => Game.Instance.TechController.CanLowerTax;


        protected override void OnInitialized()
        {
            base.OnInitialized();
            _moneyController = Game.Instance.MoneyController;
            _moneyController.GovernmentMoney
                .TakeUntilDisable(this)
                .Subscribe(OnMoneyChanged);
        }

        private void OnMoneyChanged(int money)
        {
            _moneyText.text = money.ToString();
        }

        public void RaiseSalary() => Game.Instance.ArmyManager.RaiseSalary();
        public void LowerSalary() => Game.Instance.ArmyManager.LowerSalary();

        public void RaiseTax() => Game.Instance.FolkManager.RaiseTax();
        public void LowerTax() => Game.Instance.FolkManager.LowerTax();
    }
}