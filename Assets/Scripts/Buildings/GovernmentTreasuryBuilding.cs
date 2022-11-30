#nullable enable
using NovemberProject.Buildings.UI;
using NovemberProject.CoreGameplay;
using NovemberProject.System;
using TMPro;
using UniRx;
using UnityEngine;

namespace NovemberProject.Buildings
{
    public sealed class GovernmentTreasuryBuilding : Building, IMoneyPrinter
    {
        private MoneyController _moneyController = null!;

        [SerializeField]
        private TMP_Text _moneyText = null!;

        [SerializeField]
        private int _moneyToPrint = 10;

        public override BuildingType BuildingType => BuildingType.GovernmentTreasury;
        public IReadOnlyReactiveProperty<bool> CanPrintMoney => Game.Instance.TechController.CanPrintMoney;
        public IReadOnlyReactiveProperty<bool> CanBurnMoney => Game.Instance.TechController.CanBurnMoney;

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

        public void PrintMoney() => Game.Instance.MoneyController.PrintMoney(_moneyToPrint);
        public void BurnMoney() => Game.Instance.MoneyController.BurnMoney(_moneyToPrint);
    }
}